using Microsoft.Extensions.DependencyInjection;
using PDV.Models.Pdv;
using PDV.Models.Pdv.Cadastros;
using PDV.Services.Interfaces;
using System.Globalization;
using System.IO;

namespace PDV.Services.Diagnostics
{
    public static class FiscalNfceSmokeRunner
    {
        public static int Run(ServiceProvider serviceProvider, string runRoot)
        {
            Directory.CreateDirectory(runRoot);
            var reportPath = Path.Combine(runRoot, "report.txt");
            void Log(string message) => File.AppendAllText(reportPath, $"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}");

            try
            {
                Environment.SetEnvironmentVariable("PDV_FISCAL_ENABLED", "true");
                Environment.SetEnvironmentVariable("PDV_FISCAL_MOCK", "true");
                Environment.SetEnvironmentVariable("PDV_FISCAL_AMBIENTE", "homologacao");
                Environment.SetEnvironmentVariable("PDV_SYNC_STRICT", "false");

                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PdvContext>();
                var pdv = scope.ServiceProvider.GetRequiredService<IPdvOperationService>();
                var fiscal = scope.ServiceProvider.GetRequiredService<IFiscalNfceService>();

                Seed(context);
                Log("Seed fiscal criado.");

                pdv.AbrirMovimento(100);
                Log("Caixa aberto.");

                var produto = context.Produtos.First();
                var venda = pdv.FinalizarVenda(new VendaInput(
                    new[]
                    {
                        new VendaItemDto(produto.Id, produto.Gtin ?? string.Empty, produto.DescricaoPdv ?? produto.Nome ?? "Produto", 2, ToDecimal(produto.ValorVenda), 0)
                    },
                    new[]
                    {
                        new VendaPagamentoDto("Dinheiro", ToDecimal(produto.ValorVenda) * 2)
                    },
                    NomeCliente: "Consumidor Final",
                    CpfCnpjCliente: "12345678909"));

                Log($"Venda #{venda.Id} finalizada com fiscal habilitado.");

                var cabecalho = context.NfeCabecalhos.FirstOrDefault(n => n.IdPdvVendaCabecalho == venda.Id);
                Require(cabecalho != null, "NFE_CABECALHO nao foi criado.");
                Require(cabecalho!.CodigoModelo == "65", "Modelo fiscal nao ficou NFC-e 65.");
                Require(cabecalho.StatusNota == "AUTORIZADA", $"NFC-e nao autorizada no smoke: {cabecalho.StatusNota} {cabecalho.InformacoesAddContribuinte}");
                Require(!string.IsNullOrWhiteSpace(cabecalho.ChaveAcesso) && cabecalho.ChaveAcesso.Length == 44, "Chave de acesso invalida.");
                Require(context.NfeDetalhes.Count(d => d.IdNfeCabecalho == cabecalho.Id) == 1, "NFE_DETALHE nao foi criado.");
                Require(context.NfeInformacaoPagamento.Count(p => p.IdNfeCabecalho == cabecalho.Id) == 1, "NFE_INFORMACAO_PAGAMENTO nao foi criada.");

                Log($"NFC-e OK: numero={cabecalho.Numero}, chave={cabecalho.ChaveAcesso}, status={cabecalho.StatusNota}.");

                var idempotente = fiscal.EmitirNfceVendaAsync(venda.Id!.Value).GetAwaiter().GetResult();
                Require(idempotente.Sucesso, "Reemissao idempotente falhou.");
                Require(context.NfeCabecalhos.Count(n => n.IdPdvVendaCabecalho == venda.Id) == 1, "Idempotencia criou NFC-e duplicada.");
                Log("Idempotencia fiscal OK.");

                var cancelamento = fiscal.CancelarNfceAsync(cabecalho.Id!.Value, "Cancelamento de teste fiscal automatizado").GetAwaiter().GetResult();
                Require(cancelamento.Sucesso, "Cancelamento mock falhou.");
                context.Entry(cabecalho).Reload();
                Require(cabecalho.StatusNota == "CANCELADA", "Cancelamento nao atualizou status local.");
                Log("Cancelamento fiscal OK.");

                var inutilizacao = fiscal.InutilizarNumeroAsync("1", 99, 99, "Inutilizacao de teste fiscal automatizado").GetAwaiter().GetResult();
                Require(inutilizacao.Sucesso, "Inutilizacao mock falhou.");
                Require(context.NfeNumerosInutilizados.Any(n => n.Serie == "1" && n.Numero == 99), "Inutilizacao nao gravou NFE_NUMERO_INUTILIZADO.");
                Log("Inutilizacao fiscal OK.");
                return 0;
            }
            catch (Exception ex)
            {
                Log("ERRO: " + ex);
                return 1;
            }
        }

        private static void Seed(PdvContext context)
        {
            context.Empresas.Add(new Empresa
            {
                Id = null,
                RazaoSocial = "Empresa Fiscal Smoke",
                NomeFantasia = "Empresa Fiscal Smoke",
                Cnpj = "11222333000181",
                InscricaoEstadual = "ISENTO",
                Crt = "1",
                Logradouro = "Rua Teste",
                Numero = "100",
                Bairro = "Centro",
                Cidade = "Sao Paulo",
                Uf = "SP",
                Cep = "01001000",
                Fone = "1133334444",
                CodigoIbgeUf = 35,
                CodigoIbgeCidade = 3550308,
                Registrado = true
            });

            context.ProdutosUnidades.Add(new ProdutoUnidade
            {
                Id = null,
                Sigla = "UN",
                Descricao = "Unidade"
            });
            context.SaveChanges();

            var unidadeId = context.ProdutosUnidades.First().Id;
            context.Produtos.Add(new Produto
            {
                Id = null,
                IdProdutoUnidade = unidadeId,
                Gtin = "SEM GTIN",
                CodigoInterno = "FISCAL001",
                Nome = "Produto Fiscal Smoke",
                Descricao = "Produto Fiscal Smoke",
                DescricaoPdv = "Produto Fiscal Smoke",
                ValorVenda = 12.50,
                QuantidadeEstoque = 50,
                CodigoNcm = "61091000",
                Csosn = "102",
                Situacao = "A"
            });

            context.PdvTiposPagamento.Add(new PdvTipoPagamento
            {
                Id = null,
                Codigo = "DINHEIRO",
                Descricao = "Dinheiro",
                PermiteTroco = "S",
                CodigoPagamentoNfce = "01"
            });

            context.SaveChanges();
        }

        private static decimal ToDecimal(double? value) => Convert.ToDecimal(value ?? 0, CultureInfo.InvariantCulture);

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}
