using Microsoft.EntityFrameworkCore;
using PDV.Models.Pdv.Fiscal;
using PDV.Services.Interfaces;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PDV.Services
{
    public sealed class FiscalNfceService : IFiscalNfceService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private static readonly SemaphoreSlim NumeroLock = new(1, 1);

        private readonly PdvContext _context;
        private readonly IAuthenticationService _authenticationService;

        public FiscalNfceService(PdvContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<FiscalNfceEmissionResult> EmitirNfceVendaAsync(int vendaId, bool contingencia = false)
        {
            var venda = await _context.PdvVendasCabecalho.FirstOrDefaultAsync(v => v.Id == vendaId).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Venda {vendaId} nao encontrada.");

            var existente = await _context.NfeCabecalhos
                .Where(n => n.IdPdvVendaCabecalho == vendaId)
                .OrderByDescending(n => n.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (existente != null && string.Equals(existente.StatusNota, "AUTORIZADA", StringComparison.OrdinalIgnoreCase))
            {
                return new FiscalNfceEmissionResult
                {
                    Sucesso = true,
                    NfeCabecalhoId = existente.Id,
                    Numero = existente.Numero ?? string.Empty,
                    ChaveAcesso = existente.ChaveAcesso ?? string.Empty,
                    Status = existente.StatusNota ?? string.Empty,
                    Mensagem = "NFC-e ja autorizada para esta venda.",
                    CaminhoPdf = existente.UrlChave ?? string.Empty,
                    CaminhoXml = existente.Qrcode ?? string.Empty
                };
            }

            var empresa = await ObterEmpresaAsync().ConfigureAwait(false);
            var detalhesVenda = await _context.PdvVendasDetalhe
                .Where(d => d.IdPdvVendaCabecalho == vendaId)
                .OrderBy(d => d.Item)
                .ToListAsync()
                .ConfigureAwait(false);
            var pagamentosVenda = await _context.PdvTotaisTipoPagamento
                .Where(p => p.IdPdvVendaCabecalho == vendaId)
                .ToListAsync()
                .ConfigureAwait(false);

            if (detalhesVenda.Count == 0)
                throw new InvalidOperationException("Venda sem itens para emissao fiscal.");

            if (pagamentosVenda.Count == 0)
                throw new InvalidOperationException("Venda sem pagamentos para emissao fiscal.");

            var produtos = await _context.Produtos
                .AsNoTracking()
                .Where(p => detalhesVenda.Select(d => d.IdProduto).Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p)
                .ConfigureAwait(false);

            var tiposPagamento = await _context.PdvTiposPagamento
                .AsNoTracking()
                .Where(t => pagamentosVenda.Select(p => p.IdPdvTipoPagamento).Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, t => t)
                .ConfigureAwait(false);

            var validacao = await ValidarVendaParaNfceAsync(empresa, venda, detalhesVenda, pagamentosVenda, produtos, tiposPagamento).ConfigureAwait(false);
            if (validacao.Count > 0)
                throw new InvalidOperationException("Venda nao esta pronta para NFC-e: " + string.Join("; ", validacao));

            if (!contingencia)
            {
                var statusSefaz = await ConsultarStatusSefazAsync(empresa.Uf).ConfigureAwait(false);
                if (!statusSefaz.Disponivel)
                {
                    contingencia = true;
                }
            }

            var numero = await ProximoNumeroNfceAsync("1").ConfigureAwait(false);
            var codigoNumerico = GerarCodigoNumerico(vendaId, numero);
            var chave = GerarChaveAcesso(empresa.CodigoIbgeUf ?? CodigoUf(empresa.Uf), DateTime.Now, SomenteDigitos(empresa.Cnpj), "65", "1", numero, contingencia ? "9" : "1", codigoNumerico);
            var ini = MontarIni(venda, detalhesVenda, pagamentosVenda, produtos, tiposPagamento, empresa, numero, codigoNumerico, chave, contingencia);
            var cabecalho = await PersistirDocumentoFiscalAsync(vendaId, numero, codigoNumerico, chave, venda, detalhesVenda, pagamentosVenda, produtos, tiposPagamento, contingencia).ConfigureAwait(false);

            try
            {
                var retorno = await EnviarParaRetaguardaAsync(numero, SomenteDigitos(empresa.Cnpj), ini, contingencia).ConfigureAwait(false);
                cabecalho.StatusNota = ResolverStatusNota(retorno);
                cabecalho.InformacoesAddFisco = retorno.Protocolo;
                cabecalho.InformacoesAddContribuinte = retorno.Mensagem;
                cabecalho.UrlChave = retorno.CaminhoPdf;
                cabecalho.Qrcode = retorno.CaminhoXml;
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return new FiscalNfceEmissionResult
                {
                    Sucesso = retorno.Sucesso,
                    NfeCabecalhoId = cabecalho.Id,
                    Numero = numero.ToString(CultureInfo.InvariantCulture),
                    ChaveAcesso = chave,
                    Status = cabecalho.StatusNota ?? string.Empty,
                    Mensagem = retorno.Mensagem,
                    CaminhoPdf = retorno.CaminhoPdf,
                    CaminhoXml = retorno.CaminhoXml
                };
            }
            catch (Exception ex)
            {
                cabecalho.StatusNota = "ERRO";
                cabecalho.InformacoesAddContribuinte = ex.Message;
                await _context.SaveChangesAsync().ConfigureAwait(false);

                if (FiscalStrict())
                    throw;

                return new FiscalNfceEmissionResult
                {
                    Sucesso = false,
                    NfeCabecalhoId = cabecalho.Id,
                    Numero = numero.ToString(CultureInfo.InvariantCulture),
                    ChaveAcesso = chave,
                    Status = cabecalho.StatusNota ?? string.Empty,
                    Mensagem = ex.Message
                };
            }
        }

        public async Task<FiscalNfceStatusResult> ConsultarStatusSefazAsync(string? uf = null)
        {
            if (FiscalMock())
            {
                return new FiscalNfceStatusResult
                {
                    Disponivel = true,
                    Status = "107",
                    Mensagem = "Servico em operacao no modo mock."
                };
            }

            if (_authenticationService is not AuthenticationService auth || auth.CurrentSession == null)
                throw new InvalidOperationException("Sessao da retaguarda obrigatoria para consultar status SEFAZ.");

            var cnpj = SomenteDigitos(auth.CurrentSession.Empresa?.Cnpj);
            using var http = CriarHttpClient(auth.CurrentSession.Token);
            var response = await http.GetAsync($"acbr-monitor/v2/status-sefaz?uf={Uri.EscapeDataString(uf ?? "SP")}&cnpj={Uri.EscapeDataString(cnpj)}").ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return new FiscalNfceStatusResult
                {
                    Disponivel = false,
                    Status = ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture),
                    Mensagem = content
                };
            }

            var retorno = JsonSerializer.Deserialize<RetaguardaFiscalStatusResponse>(content, JsonOptions);
            return new FiscalNfceStatusResult
            {
                Disponivel = retorno?.Disponivel ?? false,
                Status = retorno?.Status ?? string.Empty,
                Mensagem = retorno?.Mensagem ?? string.Empty
            };
        }

        public async Task<FiscalNfceEmissionResult> TransmitirContingenciaAsync(int nfeCabecalhoId)
        {
            var cabecalho = await _context.NfeCabecalhos.FirstOrDefaultAsync(n => n.Id == nfeCabecalhoId).ConfigureAwait(false)
                ?? throw new InvalidOperationException("NFC-e nao encontrada.");

            if (string.IsNullOrWhiteSpace(cabecalho.ChaveAcesso))
                throw new InvalidOperationException("NFC-e sem chave de acesso.");

            var empresa = await ObterEmpresaAsync().ConfigureAwait(false);
            var retorno = await AcaoFiscalAsync("acbr-monitor/v2/transmite-contingencia", new RetaguardaFiscalActionRequest
            {
                Cnpj = SomenteDigitos(empresa.Cnpj),
                ChaveAcesso = cabecalho.ChaveAcesso
            }).ConfigureAwait(false);

            cabecalho.StatusNota = ResolverStatusNota(retorno);
            cabecalho.InformacoesAddContribuinte = retorno.Mensagem;
            cabecalho.UrlChave = retorno.CaminhoPdf;
            cabecalho.Qrcode = retorno.CaminhoXml;
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return MapRetorno(cabecalho, retorno);
        }

        public async Task<FiscalNfceEmissionResult> CancelarNfceAsync(int nfeCabecalhoId, string justificativa)
        {
            if (string.IsNullOrWhiteSpace(justificativa) || justificativa.Trim().Length < 15)
                throw new InvalidOperationException("Justificativa de cancelamento deve ter pelo menos 15 caracteres.");

            var cabecalho = await _context.NfeCabecalhos.FirstOrDefaultAsync(n => n.Id == nfeCabecalhoId).ConfigureAwait(false)
                ?? throw new InvalidOperationException("NFC-e nao encontrada.");

            var empresa = await ObterEmpresaAsync().ConfigureAwait(false);
            var retorno = await AcaoFiscalAsync("acbr-monitor/v2/cancela-nfce", new RetaguardaFiscalActionRequest
            {
                Cnpj = SomenteDigitos(empresa.Cnpj),
                ChaveAcesso = cabecalho.ChaveAcesso,
                Justificativa = justificativa
            }).ConfigureAwait(false);

            cabecalho.StatusNota = retorno.Sucesso ? "CANCELADA" : "ERRO_CANCELAMENTO";
            cabecalho.InformacoesAddContribuinte = retorno.Mensagem;
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return MapRetorno(cabecalho, retorno);
        }

        public async Task<FiscalNfceEmissionResult> InutilizarNumeroAsync(string serie, int numeroInicial, int numeroFinal, string justificativa)
        {
            if (numeroInicial <= 0 || numeroFinal < numeroInicial)
                throw new InvalidOperationException("Faixa de inutilizacao invalida.");

            if (string.IsNullOrWhiteSpace(justificativa) || justificativa.Trim().Length < 15)
                throw new InvalidOperationException("Justificativa de inutilizacao deve ter pelo menos 15 caracteres.");

            var empresa = await ObterEmpresaAsync().ConfigureAwait(false);
            var retorno = await AcaoFiscalAsync("acbr-monitor/v2/inutiliza-numero", new RetaguardaFiscalActionRequest
            {
                Cnpj = SomenteDigitos(empresa.Cnpj),
                Modelo = "65",
                Serie = serie,
                NumeroInicial = numeroInicial.ToString(CultureInfo.InvariantCulture),
                NumeroFinal = numeroFinal.ToString(CultureInfo.InvariantCulture),
                Justificativa = justificativa,
                Ano = DateTime.Today.ToString("yy", CultureInfo.InvariantCulture)
            }).ConfigureAwait(false);

            if (retorno.Sucesso)
            {
                for (var numero = numeroInicial; numero <= numeroFinal; numero++)
                {
                    if (!await _context.NfeNumerosInutilizados.AnyAsync(n => n.Serie == serie && n.Numero == numero).ConfigureAwait(false))
                    {
                        _context.NfeNumerosInutilizados.Add(new NfeNumeroInutilizado
                        {
                            Id = null,
                            Serie = serie,
                            Numero = numero,
                            DataInutilizacao = DateTime.Today,
                            Observacao = justificativa
                        });
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return new FiscalNfceEmissionResult
            {
                Sucesso = retorno.Sucesso,
                Numero = $"{numeroInicial}-{numeroFinal}",
                Status = retorno.Status,
                Mensagem = retorno.Mensagem
            };
        }

        private async Task<Models.Pdv.Cadastros.Empresa> ObterEmpresaAsync()
        {
            var cnpjSessao = ObterCnpjSessao();
            var query = _context.Empresas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(cnpjSessao))
                query = query.Where(e => e.Cnpj == cnpjSessao);

            var empresa = await query.OrderBy(e => e.Id).FirstOrDefaultAsync().ConfigureAwait(false)
                ?? await _context.Empresas.OrderBy(e => e.Id).FirstOrDefaultAsync().ConfigureAwait(false);

            if (empresa == null)
                throw new InvalidOperationException("Cadastre a empresa antes de emitir NFC-e.");

            if (string.IsNullOrWhiteSpace(SomenteDigitos(empresa.Cnpj)))
                throw new InvalidOperationException("Empresa sem CNPJ valido para NFC-e.");

            return empresa;
        }

        private async Task<int> ProximoNumeroNfceAsync(string serie)
        {
            await NumeroLock.WaitAsync().ConfigureAwait(false);
            try
            {
                var registro = await _context.NfeNumeros
                    .FirstOrDefaultAsync(n => n.Modelo == "65" && n.Serie == serie)
                    .ConfigureAwait(false);

                if (registro == null)
                {
                    var ultimo = await _context.NfeCabecalhos
                        .Where(n => n.CodigoModelo == "65" && n.Serie == serie)
                        .Select(n => n.Numero)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var max = ultimo
                        .Select(n => int.TryParse(n, out var parsed) ? parsed : 0)
                        .DefaultIfEmpty(0)
                        .Max();

                    registro = new NfeNumero
                    {
                        Id = null,
                        Modelo = "65",
                        Serie = serie,
                        Numero = max
                    };
                    _context.NfeNumeros.Add(registro);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }

                registro.Numero = (registro.Numero ?? 0) + 1;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return registro.Numero.Value;
            }
            finally
            {
                NumeroLock.Release();
            }
        }

        private async Task<List<string>> ValidarVendaParaNfceAsync(
            Models.Pdv.Cadastros.Empresa empresa,
            Models.Pdv.PdvVendaCabecalho venda,
            IReadOnlyList<Models.Pdv.PdvVendaDetalhe> detalhesVenda,
            IReadOnlyList<Models.Pdv.PdvTotalTipoPagamento> pagamentosVenda,
            IReadOnlyDictionary<int?, Models.Pdv.Cadastros.Produto> produtos,
            IReadOnlyDictionary<int?, Models.Pdv.PdvTipoPagamento> tiposPagamento)
        {
            var erros = new List<string>();

            if (SomenteDigitos(empresa.Cnpj).Length != 14)
                erros.Add("empresa sem CNPJ com 14 digitos");
            if (string.IsNullOrWhiteSpace(empresa.RazaoSocial))
                erros.Add("empresa sem razao social");
            if (string.IsNullOrWhiteSpace(empresa.Uf))
                erros.Add("empresa sem UF");
            if (!empresa.CodigoIbgeCidade.HasValue || empresa.CodigoIbgeCidade <= 0)
                erros.Add("empresa sem codigo IBGE do municipio");
            if (string.IsNullOrWhiteSpace(empresa.Crt))
                erros.Add("empresa sem CRT/regime tributario");
            if (string.IsNullOrWhiteSpace(empresa.Logradouro) || string.IsNullOrWhiteSpace(empresa.Bairro) || string.IsNullOrWhiteSpace(empresa.Cidade))
                erros.Add("empresa com endereco incompleto");

            var configuracao = await _context.NfeConfiguracoes.AsNoTracking().OrderBy(c => c.Id).FirstOrDefaultAsync().ConfigureAwait(false);
            if (!FiscalMock() && configuracao != null)
            {
                if (string.IsNullOrWhiteSpace(configuracao.NfceIdCsc))
                    erros.Add("configuracao NFC-e sem IdToken/CSC ID");
                if (string.IsNullOrWhiteSpace(configuracao.NfceCsc))
                    erros.Add("configuracao NFC-e sem CSC");
            }

            if ((venda.ValorFinal ?? 0) <= 0)
                erros.Add("valor total da venda deve ser maior que zero");
            if ((venda.ValorRecebido ?? 0) < (venda.ValorFinal ?? 0))
                erros.Add("valor recebido menor que valor final");

            foreach (var item in detalhesVenda)
            {
                produtos.TryGetValue(item.IdProduto, out var produto);
                var nomeProduto = produto?.DescricaoPdv ?? produto?.Nome ?? $"item {item.Item}";
                if (!item.Quantidade.HasValue || item.Quantidade <= 0)
                    erros.Add($"{nomeProduto}: quantidade invalida");
                if (!item.ValorUnitario.HasValue || item.ValorUnitario <= 0)
                    erros.Add($"{nomeProduto}: valor unitario invalido");
                if (NormalizarNcm(produto?.CodigoNcm) == "00000000" && !FiscalMock())
                    erros.Add($"{nomeProduto}: NCM invalido");
                if (string.IsNullOrWhiteSpace(produto?.Csosn) && string.IsNullOrWhiteSpace(produto?.Cst))
                    erros.Add($"{nomeProduto}: CST/CSOSN nao informado");
            }

            foreach (var pagamento in pagamentosVenda)
            {
                tiposPagamento.TryGetValue(pagamento.IdPdvTipoPagamento, out var tipo);
                if ((pagamento.Valor ?? 0) <= 0)
                    erros.Add("pagamento com valor invalido");
                if (ResolverCodigoPagamentoNfce(tipo?.CodigoPagamentoNfce, tipo?.Descricao) == "99" && !FiscalMock())
                    erros.Add($"{tipo?.Descricao ?? "pagamento"}: codigo NFC-e nao configurado");
            }

            return erros;
        }

        private async Task<NfeCabecalho> PersistirDocumentoFiscalAsync(
            int vendaId,
            int numero,
            string codigoNumerico,
            string chave,
            Models.Pdv.PdvVendaCabecalho venda,
            IReadOnlyList<Models.Pdv.PdvVendaDetalhe> detalhesVenda,
            IReadOnlyList<Models.Pdv.PdvTotalTipoPagamento> pagamentosVenda,
            IReadOnlyDictionary<int?, Models.Pdv.Cadastros.Produto> produtos,
            IReadOnlyDictionary<int?, Models.Pdv.PdvTipoPagamento> tiposPagamento,
            bool contingencia)
        {
            var agora = DateTime.Now;
            var cabecalho = new NfeCabecalho
            {
                Id = null,
                IdPdvVendaCabecalho = vendaId,
                UfEmitente = 0,
                CodigoNumerico = codigoNumerico,
                NaturezaOperacao = "VENDA AO CONSUMIDOR",
                CodigoModelo = "65",
                Serie = "1",
                Numero = numero.ToString(CultureInfo.InvariantCulture),
                DataHoraEmissao = agora,
                DataHoraEntradaSaida = agora,
                TipoOperacao = "1",
                LocalDestino = "1",
                FormatoImpressaoDanfe = "4",
                TipoEmissao = contingencia ? "9" : "1",
                ChaveAcesso = chave,
                Ambiente = AmbienteFiscal(),
                FinalidadeEmissao = "1",
                ConsumidorOperacao = "1",
                ConsumidorPresenca = "1",
                ProcessoEmissao = "0",
                VersaoProcessoEmissao = "SynfluxPDV",
                ValorTotalProdutos = venda.ValorTotalProdutos,
                ValorDesconto = venda.ValorDesconto,
                ValorTotal = venda.ValorFinal,
                ValorTotalTributos = 0,
                StatusNota = "PENDENTE"
            };

            _context.NfeCabecalhos.Add(cabecalho);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            foreach (var item in detalhesVenda)
            {
                produtos.TryGetValue(item.IdProduto, out var produto);
                _context.NfeDetalhes.Add(new NfeDetalhe
                {
                    Id = null,
                    IdNfeCabecalho = cabecalho.Id,
                    NumeroItem = item.Item,
                    CodigoProduto = produto?.CodigoInterno ?? item.IdProduto?.ToString() ?? item.Item?.ToString(),
                    Gtin = string.IsNullOrWhiteSpace(item.Gtin) ? "SEM GTIN" : item.Gtin,
                    NomeProduto = produto?.DescricaoPdv ?? produto?.Nome ?? produto?.Descricao ?? $"Item {item.Item}",
                    Ncm = NormalizarNcm(produto?.CodigoNcm),
                    Cest = produto?.CodigoCest,
                    Cfop = item.Cfop ?? 5102,
                    UnidadeComercial = "UN",
                    QuantidadeComercial = item.Quantidade,
                    ValorUnitarioComercial = item.ValorUnitario,
                    ValorBrutoProduto = item.ValorTotal,
                    GtinUnidadeTributavel = string.IsNullOrWhiteSpace(item.Gtin) ? "SEM GTIN" : item.Gtin,
                    UnidadeTributavel = "UN",
                    QuantidadeTributavel = item.Quantidade,
                    ValorUnitarioTributavel = item.ValorUnitario,
                    ValorDesconto = item.ValorDesconto,
                    EntraTotal = "1",
                    ValorTotalTributos = 0,
                    ValorSubtotal = item.ValorTotal,
                    ValorTotal = item.ValorTotalItem
                });
            }

            foreach (var pagamento in pagamentosVenda)
            {
                tiposPagamento.TryGetValue(pagamento.IdPdvTipoPagamento, out var tipoPagamento);
                _context.NfeInformacaoPagamento.Add(new NfeInformacaoPagamento
                {
                    Id = null,
                    IdNfeCabecalho = cabecalho.Id,
                    IndicadorPagamento = "0",
                    MeioPagamento = ResolverCodigoPagamentoNfce(tipoPagamento?.CodigoPagamentoNfce, tipoPagamento?.Descricao),
                    Valor = pagamento.Valor,
                    TipoIntegracao = string.IsNullOrWhiteSpace(pagamento.Nsu) ? null : "1",
                    NumeroAutorizacao = pagamento.Nsu,
                    Troco = venda.ValorTroco
                });
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return cabecalho;
        }

        private string MontarIni(
            Models.Pdv.PdvVendaCabecalho venda,
            IReadOnlyList<Models.Pdv.PdvVendaDetalhe> detalhesVenda,
            IReadOnlyList<Models.Pdv.PdvTotalTipoPagamento> pagamentosVenda,
            IReadOnlyDictionary<int?, Models.Pdv.Cadastros.Produto> produtos,
            IReadOnlyDictionary<int?, Models.Pdv.PdvTipoPagamento> tiposPagamento,
            Models.Pdv.Cadastros.Empresa empresa,
            int numero,
            string codigoNumerico,
            string chave,
            bool contingencia)
        {
            var sb = new StringBuilder();
            var agora = DateTime.Now;
            var cnpj = SomenteDigitos(empresa.Cnpj);
            var codigoUf = empresa.CodigoIbgeUf ?? CodigoUf(empresa.Uf);
            var codigoMunicipio = empresa.CodigoIbgeCidade ?? 3550308;

            sb.AppendLine("[infNFe]");
            sb.AppendLine("versao=4.00");
            sb.AppendLine();
            sb.AppendLine("[Identificacao]");
            Append(sb, "cUF", codigoUf);
            Append(sb, "cNF", codigoNumerico);
            Append(sb, "natOp", "VENDA AO CONSUMIDOR");
            Append(sb, "mod", "65");
            Append(sb, "serie", "1");
            Append(sb, "nNF", numero);
            Append(sb, "dhEmi", agora.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            Append(sb, "tpNF", "1");
            Append(sb, "idDest", "1");
            Append(sb, "cMunFG", codigoMunicipio);
            Append(sb, "tpImp", "4");
            Append(sb, "tpEmis", contingencia ? "9" : "1");
            Append(sb, "tpAmb", AmbienteFiscal());
            Append(sb, "finNFe", "1");
            Append(sb, "indFinal", "1");
            Append(sb, "indPres", "1");
            Append(sb, "procEmi", "0");
            Append(sb, "verProc", "SynfluxPDV");
            sb.AppendLine();

            sb.AppendLine("[Emitente]");
            Append(sb, "CNPJ", cnpj);
            Append(sb, "xNome", empresa.RazaoSocial ?? empresa.NomeFantasia ?? "Empresa PDV");
            Append(sb, "xFant", empresa.NomeFantasia ?? empresa.RazaoSocial ?? "Empresa PDV");
            Append(sb, "IE", string.IsNullOrWhiteSpace(empresa.InscricaoEstadual) ? "ISENTO" : empresa.InscricaoEstadual);
            Append(sb, "CRT", empresa.Crt ?? "1");
            Append(sb, "xLgr", empresa.Logradouro ?? "Endereco nao informado");
            Append(sb, "nro", empresa.Numero ?? "S/N");
            Append(sb, "xBairro", empresa.Bairro ?? "Centro");
            Append(sb, "cMun", codigoMunicipio);
            Append(sb, "xMun", empresa.Cidade ?? "Cidade");
            Append(sb, "UF", empresa.Uf ?? "SP");
            Append(sb, "CEP", SomenteDigitos(empresa.Cep).PadRight(8, '0')[..8]);
            Append(sb, "cPais", "1058");
            Append(sb, "xPais", "BRASIL");
            Append(sb, "fone", SomenteDigitos(empresa.Fone));
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(venda.CpfCnpjCliente))
            {
                var doc = SomenteDigitos(venda.CpfCnpjCliente);
                sb.AppendLine("[Destinatario]");
                Append(sb, doc.Length > 11 ? "CNPJ" : "CPF", doc);
                Append(sb, "xNome", venda.NomeCliente ?? "Consumidor");
                Append(sb, "indIEDest", "9");
                sb.AppendLine();
            }

            foreach (var item in detalhesVenda)
            {
                produtos.TryGetValue(item.IdProduto, out var produto);
                var numeroItem = item.Item ?? 1;
                var gtin = string.IsNullOrWhiteSpace(item.Gtin) ? "SEM GTIN" : item.Gtin;
                sb.AppendLine($"[Produto{numeroItem:000}]");
                Append(sb, "cProd", produto?.CodigoInterno ?? item.IdProduto?.ToString() ?? numeroItem.ToString(CultureInfo.InvariantCulture));
                Append(sb, "cEAN", gtin);
                Append(sb, "xProd", produto?.DescricaoPdv ?? produto?.Nome ?? item.Gtin ?? $"Item {numeroItem}");
                Append(sb, "NCM", NormalizarNcm(produto?.CodigoNcm));
                Append(sb, "CFOP", item.Cfop ?? 5102);
                Append(sb, "uCom", "UN");
                Append(sb, "qCom", Numero(item.Quantidade));
                Append(sb, "vUnCom", Numero(item.ValorUnitario));
                Append(sb, "vProd", Numero(item.ValorTotal));
                Append(sb, "cEANTrib", gtin);
                Append(sb, "uTrib", "UN");
                Append(sb, "qTrib", Numero(item.Quantidade));
                Append(sb, "vUnTrib", Numero(item.ValorUnitario));
                Append(sb, "vDesc", Numero(item.ValorDesconto));
                Append(sb, "indTot", "1");
                sb.AppendLine();

                sb.AppendLine($"[ICMS{numeroItem:000}]");
                Append(sb, "CSOSN", produto?.Csosn ?? "102");
                Append(sb, "orig", "0");
                sb.AppendLine();

                sb.AppendLine($"[PIS{numeroItem:000}]");
                Append(sb, "CST", "49");
                Append(sb, "vBC", "0.00");
                Append(sb, "pPIS", "0.00");
                Append(sb, "vPIS", "0.00");
                sb.AppendLine();

                sb.AppendLine($"[COFINS{numeroItem:000}]");
                Append(sb, "CST", "49");
                Append(sb, "vBC", "0.00");
                Append(sb, "pCOFINS", "0.00");
                Append(sb, "vCOFINS", "0.00");
                sb.AppendLine();
            }

            sb.AppendLine("[Total]");
            Append(sb, "vBC", "0.00");
            Append(sb, "vICMS", "0.00");
            Append(sb, "vProd", Numero(venda.ValorTotalProdutos));
            Append(sb, "vDesc", Numero(venda.ValorDesconto));
            Append(sb, "vPIS", "0.00");
            Append(sb, "vCOFINS", "0.00");
            Append(sb, "vNF", Numero(venda.ValorFinal));
            Append(sb, "vTotTrib", "0.00");
            sb.AppendLine();

            sb.AppendLine("[Transportador]");
            Append(sb, "modFrete", "9");
            sb.AppendLine();

            var indicePagamento = 1;
            foreach (var pagamento in pagamentosVenda)
            {
                tiposPagamento.TryGetValue(pagamento.IdPdvTipoPagamento, out var tipoPagamento);
                sb.AppendLine($"[pag{indicePagamento:000}]");
                Append(sb, "indPag", "0");
                Append(sb, "tPag", ResolverCodigoPagamentoNfce(tipoPagamento?.CodigoPagamentoNfce, tipoPagamento?.Descricao));
                Append(sb, "vPag", Numero(pagamento.Valor));
                if (!string.IsNullOrWhiteSpace(pagamento.Nsu))
                    Append(sb, "cAut", pagamento.Nsu);
                sb.AppendLine();
                indicePagamento++;
            }

            sb.AppendLine("[DadosAdicionais]");
            Append(sb, "infCpl", $"Venda PDV {venda.Id}. Chave prevista {chave}.");

            return sb.ToString();
        }

        private async Task<RetaguardaFiscalResponse> EnviarParaRetaguardaAsync(int numero, string cnpj, string nfceIni, bool contingencia)
        {
            if (FiscalMock())
            {
                return new RetaguardaFiscalResponse
                {
                    Sucesso = true,
                    Status = "AUTORIZADA",
                    Mensagem = "Emissao fiscal simulada em ambiente local.",
                    CaminhoPdf = $"mock://nfce/{cnpj}/{numero}.pdf",
                    CaminhoXml = $"mock://nfce/{cnpj}/{numero}.xml"
                };
            }

            if (_authenticationService is not AuthenticationService auth || auth.CurrentSession == null)
                throw new InvalidOperationException("Sessao da retaguarda obrigatoria para emitir NFC-e.");

            using var http = CriarHttpClient(auth.CurrentSession.Token);
            var body = new RetaguardaFiscalRequest
            {
                Numero = numero.ToString(CultureInfo.InvariantCulture),
                Cnpj = cnpj,
                NfceIniBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(nfceIni)),
                Contingencia = contingencia
            };

            var response = await http.PostAsJsonAsync("acbr-monitor/v2/emite-nfce", body, JsonOptions).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Retaguarda fiscal retornou erro {(int)response.StatusCode}: {content}");

            return JsonSerializer.Deserialize<RetaguardaFiscalResponse>(content, JsonOptions)
                ?? throw new InvalidOperationException("Retaguarda fiscal retornou resposta vazia.");
        }

        private async Task<RetaguardaFiscalResponse> AcaoFiscalAsync(string endpoint, RetaguardaFiscalActionRequest request)
        {
            if (FiscalMock())
            {
                return new RetaguardaFiscalResponse
                {
                    Sucesso = true,
                    Status = "AUTORIZADA",
                    Mensagem = "Acao fiscal simulada em ambiente local.",
                    Protocolo = "MOCK"
                };
            }

            if (_authenticationService is not AuthenticationService auth || auth.CurrentSession == null)
                throw new InvalidOperationException("Sessao da retaguarda obrigatoria para acao fiscal.");

            using var http = CriarHttpClient(auth.CurrentSession.Token);
            var response = await http.PostAsJsonAsync(endpoint, request, JsonOptions).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Retaguarda fiscal retornou erro {(int)response.StatusCode}: {content}");

            return JsonSerializer.Deserialize<RetaguardaFiscalResponse>(content, JsonOptions)
                ?? throw new InvalidOperationException("Retaguarda fiscal retornou resposta vazia.");
        }

        private static FiscalNfceEmissionResult MapRetorno(NfeCabecalho cabecalho, RetaguardaFiscalResponse retorno)
        {
            return new FiscalNfceEmissionResult
            {
                Sucesso = retorno.Sucesso,
                NfeCabecalhoId = cabecalho.Id,
                Numero = cabecalho.Numero ?? string.Empty,
                ChaveAcesso = cabecalho.ChaveAcesso ?? string.Empty,
                Status = cabecalho.StatusNota ?? retorno.Status,
                Mensagem = retorno.Mensagem,
                CaminhoPdf = retorno.CaminhoPdf,
                CaminhoXml = retorno.CaminhoXml
            };
        }

        private static string ResolverStatusNota(RetaguardaFiscalResponse retorno)
        {
            if (retorno.Sucesso)
                return string.IsNullOrWhiteSpace(retorno.Status) ? "AUTORIZADA" : retorno.Status;

            if ((retorno.Mensagem ?? string.Empty).Contains("reje", StringComparison.OrdinalIgnoreCase) ||
                (retorno.Status ?? string.Empty).Contains("reje", StringComparison.OrdinalIgnoreCase))
                return "REJEITADA";

            return "ERRO";
        }

        private static HttpClient CriarHttpClient(string token)
        {
            var baseUrl = RetaguardaEndpointResolver.ObterBaseUrl();

            var http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"),
                Timeout = TimeSpan.FromSeconds(120)
            };
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return http;
        }

        private string ObterCnpjSessao()
        {
            if (_authenticationService is AuthenticationService auth && auth.CurrentSession != null)
                return SomenteDigitos(auth.CurrentSession.Empresa?.Cnpj);

            return SomenteDigitos(Environment.GetEnvironmentVariable("PDV_EMPRESA_CNPJ"));
        }

        private static string ResolverCodigoPagamentoNfce(string? codigoConfigurado, string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(codigoConfigurado))
                return codigoConfigurado.Trim();

            descricao ??= string.Empty;
            if (descricao.Contains("Dinheiro", StringComparison.OrdinalIgnoreCase))
                return "01";
            if (descricao.Contains("Credito", StringComparison.OrdinalIgnoreCase) || descricao.Contains("Crédito", StringComparison.OrdinalIgnoreCase))
                return "03";
            if (descricao.Contains("Debito", StringComparison.OrdinalIgnoreCase) || descricao.Contains("Débito", StringComparison.OrdinalIgnoreCase))
                return "04";
            if (descricao.Contains("Pix", StringComparison.OrdinalIgnoreCase))
                return "17";
            if (descricao.Contains("Fiado", StringComparison.OrdinalIgnoreCase) || descricao.Contains("Prazo", StringComparison.OrdinalIgnoreCase))
                return "05";

            return "99";
        }

        private static string GerarCodigoNumerico(int vendaId, int numero)
        {
            var value = Math.Abs(HashCode.Combine(vendaId, numero, DateTime.Today));
            return (value % 100000000).ToString("00000000", CultureInfo.InvariantCulture);
        }

        private static string GerarChaveAcesso(int uf, DateTime emissao, string cnpj, string modelo, string serie, int numero, string tipoEmissao, string codigoNumerico)
        {
            var baseChave =
                uf.ToString("00", CultureInfo.InvariantCulture) +
                emissao.ToString("yyMM") +
                cnpj.PadLeft(14, '0')[..14] +
                modelo.PadLeft(2, '0') +
                serie.PadLeft(3, '0') +
                numero.ToString("000000000", CultureInfo.InvariantCulture) +
                tipoEmissao +
                codigoNumerico.PadLeft(8, '0')[..8];

            return baseChave + CalcularDigitoModulo11(baseChave);
        }

        private static int CalcularDigitoModulo11(string chave)
        {
            var peso = 2;
            var soma = 0;
            for (var i = chave.Length - 1; i >= 0; i--)
            {
                soma += (chave[i] - '0') * peso;
                peso++;
                if (peso > 9)
                    peso = 2;
            }

            var resto = soma % 11;
            var digito = 11 - resto;
            return digito >= 10 ? 0 : digito;
        }

        private static int CodigoUf(string? uf)
        {
            return (uf ?? "SP").Trim().ToUpperInvariant() switch
            {
                "RO" => 11, "AC" => 12, "AM" => 13, "RR" => 14, "PA" => 15, "AP" => 16, "TO" => 17,
                "MA" => 21, "PI" => 22, "CE" => 23, "RN" => 24, "PB" => 25, "PE" => 26, "AL" => 27, "SE" => 28, "BA" => 29,
                "MG" => 31, "ES" => 32, "RJ" => 33, "SP" => 35,
                "PR" => 41, "SC" => 42, "RS" => 43,
                "MS" => 50, "MT" => 51, "GO" => 52, "DF" => 53,
                _ => 35
            };
        }

        private static string AmbienteFiscal()
        {
            var ambiente = Environment.GetEnvironmentVariable("PDV_FISCAL_AMBIENTE");
            return string.Equals(ambiente, "1", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ambiente, "producao", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ambiente, "produção", StringComparison.OrdinalIgnoreCase)
                ? "1"
                : "2";
        }

        private static bool FiscalStrict()
        {
            return string.Equals(Environment.GetEnvironmentVariable("PDV_FISCAL_STRICT"), "true", StringComparison.OrdinalIgnoreCase);
        }

        private static bool FiscalMock()
        {
            return string.Equals(Environment.GetEnvironmentVariable("PDV_FISCAL_MOCK"), "true", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizarNcm(string? value)
        {
            var ncm = SomenteDigitos(value);
            return string.IsNullOrWhiteSpace(ncm) ? "00000000" : ncm.PadRight(8, '0')[..8];
        }

        private static string SomenteDigitos(string? value)
        {
            return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        }

        private static string Numero(double? value)
        {
            return (value ?? 0).ToString("0.00######", CultureInfo.InvariantCulture);
        }

        private static void Append(StringBuilder sb, string key, object? value)
        {
            sb.Append(key).Append('=').Append(value?.ToString() ?? string.Empty).AppendLine();
        }

        private sealed class RetaguardaFiscalRequest
        {
            [JsonPropertyName("numero")]
            public string Numero { get; set; } = string.Empty;

            [JsonPropertyName("cnpj")]
            public string Cnpj { get; set; } = string.Empty;

            [JsonPropertyName("nfceIniBase64")]
            public string NfceIniBase64 { get; set; } = string.Empty;

            [JsonPropertyName("contingencia")]
            public bool Contingencia { get; set; }
        }

        private sealed class RetaguardaFiscalResponse
        {
            [JsonPropertyName("sucesso")]
            public bool Sucesso { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;

            [JsonPropertyName("mensagem")]
            public string Mensagem { get; set; } = string.Empty;

            [JsonPropertyName("caminhoPdf")]
            public string CaminhoPdf { get; set; } = string.Empty;

            [JsonPropertyName("caminhoXml")]
            public string CaminhoXml { get; set; } = string.Empty;

            [JsonPropertyName("protocolo")]
            public string Protocolo { get; set; } = string.Empty;
        }

        private sealed class RetaguardaFiscalStatusResponse
        {
            [JsonPropertyName("disponivel")]
            public bool Disponivel { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;

            [JsonPropertyName("mensagem")]
            public string Mensagem { get; set; } = string.Empty;
        }

        private sealed class RetaguardaFiscalActionRequest
        {
            [JsonPropertyName("cnpj")]
            public string Cnpj { get; set; } = string.Empty;

            [JsonPropertyName("chaveAcesso")]
            public string? ChaveAcesso { get; set; }

            [JsonPropertyName("justificativa")]
            public string? Justificativa { get; set; }

            [JsonPropertyName("ano")]
            public string? Ano { get; set; }

            [JsonPropertyName("modelo")]
            public string? Modelo { get; set; }

            [JsonPropertyName("serie")]
            public string? Serie { get; set; }

            [JsonPropertyName("numeroInicial")]
            public string? NumeroInicial { get; set; }

            [JsonPropertyName("numeroFinal")]
            public string? NumeroFinal { get; set; }
        }
    }
}
