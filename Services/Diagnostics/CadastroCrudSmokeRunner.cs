using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PDV.Services.Interfaces;

namespace PDV.Services.Diagnostics
{
    public static class CadastroCrudSmokeRunner
    {
        private static readonly string[] CadastroKeys =
        {
            "cliente",
            "fornecedor",
            "colaborador",
            "empresa",
            "produto",
            "produto_unidade",
            "forma_pagamento",
            "cliente_fiado",
            "nfce_plano_pagamento",
            "taxa_entrega",
            "cardapio",
            "cardapio_pergunta",
            "cardapio_resposta",
            "cozinha",
            "mesa",
            "produto_grupo",
            "produto_subgrupo",
            "produto_tipo",
            "produto_imagem",
            "produto_promocao",
            "produto_ficha_tecnica",
            "contador",
            "pdv_configuracao",
            "pdv_configuracao_balanca",
            "pdv_configuracao_leitor_serial",
            "pdv_operador",
            "pdv_tipo_pagamento",
            "cfop",
            "ecf_aliquotas",
            "ecf_impressora",
            "nfe_configuracao",
            "nfe_responsavel_tecnico",
            "tribut_grupo",
            "tribut_operacao",
            "tribut_icms",
            "tribut_pis",
            "tribut_cofins",
            "tribut_ipi",
            "tribut_iss",
            "ibpt"
        };

        public static int Run(ServiceProvider serviceProvider, string runRoot)
        {
            Directory.CreateDirectory(runRoot);
            var reportPath = Path.Combine(runRoot, "cadastro-crud-smoke.txt");
            var linhas = new List<string>
            {
                $"Inicio: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"Total esperado: {CadastroKeys.Length}"
            };

            try
            {
                using var scope = serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IPdvFeatureService>();

                foreach (var key in CadastroKeys)
                {
                    var tela = service.ObterTela(key);
                    var layout = service.ObterLayoutListagem(key);
                    var novo = service.CriarModeloEdicao(key);
                    Preencher(novo, key, "novo");
                    var id = service.SalvarRegistro(key, null, novo.ToDictionary());
                    if (id <= 0)
                        throw new InvalidOperationException($"{key}: criacao nao retornou Id valido.");

                    var aposCriacao = service.CarregarLinhas(key).FirstOrDefault(r => r.Id == id);
                    if (aposCriacao == null)
                        throw new InvalidOperationException($"{key}: registro criado nao apareceu na listagem.");

                    var edicao = service.CriarModeloEdicao(key, id);
                    Preencher(edicao, key, "editado");
                    service.SalvarRegistro(key, id, edicao.ToDictionary());

                    var aposEdicao = service.CarregarLinhas(key).FirstOrDefault(r => r.Id == id);
                    if (aposEdicao == null)
                        throw new InvalidOperationException($"{key}: registro editado sumiu antes da exclusao.");

                    service.ExcluirRegistro(key, id);

                    var aposExclusao = service.CarregarLinhas(key).FirstOrDefault(r => r.Id == id);
                    if (aposExclusao != null)
                        throw new InvalidOperationException($"{key}: registro ainda aparece depois da exclusao.");

                    linhas.Add($"OK | {key} | {tela.TableName} | criar/editar/excluir | colunas: {DescreverLayout(layout)}");
                }

                ValidarLayoutsCriticos(service, linhas);

                linhas.Add($"Fim: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                linhas.Add("RESULTADO: OK");
                File.WriteAllLines(reportPath, linhas);
                return 0;
            }
            catch (Exception ex)
            {
                linhas.Add($"ERRO: {ex}");
                linhas.Add("RESULTADO: FALHOU");
                File.WriteAllLines(reportPath, linhas);
                return 1;
            }
        }

        private static void Preencher(PdvFeatureEditModel model, string key, string sufixo)
        {
            foreach (var campo in model.Campos)
            {
                campo.ValorTexto = campo.Tipo switch
                {
                    "Data" => DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture),
                    "Numero" => campo.PropertyName.StartsWith("Id", StringComparison.OrdinalIgnoreCase) ? "1" : (sufixo == "novo" ? "1" : "2"),
                    "Booleano" => "N",
                    _ => ValorTexto(key, campo.PropertyName, sufixo)
                };
            }
        }

        private static string ValorTexto(string key, string propertyName, string sufixo)
        {
            if (propertyName.Contains("Email", StringComparison.OrdinalIgnoreCase))
                return $"{key}.{sufixo}@local.com";
            if (propertyName.Contains("Cnpj", StringComparison.OrdinalIgnoreCase))
                return "11222333000181";
            if (propertyName.Contains("Cpf", StringComparison.OrdinalIgnoreCase))
                return "12345678910";
            if (propertyName.Equals("Uf", StringComparison.OrdinalIgnoreCase))
                return "SP";
            if (propertyName.Contains("Status", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Situacao", StringComparison.OrdinalIgnoreCase))
                return sufixo == "novo" ? "Ativo" : "Editado";

            return $"{key}-{propertyName}-{sufixo}";
        }

        private static void ValidarLayoutsCriticos(IPdvFeatureService service, List<string> linhas)
        {
            var operador = service.ObterLayoutListagem("pdv_operador");
            if (operador.MostrarValor)
                throw new InvalidOperationException("Layout invalido: Operador PDV nao deve exibir coluna Valor.");

            if (operador.MostrarData)
                throw new InvalidOperationException("Layout invalido: Operador PDV nao deve exibir coluna Data sem campo real correspondente.");

            var clienteFiado = service.ObterLayoutListagem("cliente_fiado");
            if (!clienteFiado.MostrarValor)
                throw new InvalidOperationException("Layout invalido: Cliente Fiado deve exibir coluna Valor.");

            linhas.Add("OK | layout critico | Operador PDV sem Valor/Data; Cliente Fiado com Valor");
        }

        private static string DescreverLayout(PdvFeatureColumnLayout layout)
        {
            var colunas = new List<string> { "Id" };
            if (layout.MostrarCodigo) colunas.Add("Codigo");
            if (layout.MostrarNome) colunas.Add("Nome");
            if (layout.MostrarDescricao) colunas.Add("Descricao");
            if (layout.MostrarStatus) colunas.Add("Status");
            if (layout.MostrarValor) colunas.Add("Valor");
            if (layout.MostrarData) colunas.Add("Data");
            return string.Join(",", colunas);
        }
    }
}
