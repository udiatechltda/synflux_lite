using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PDV.Services.Interfaces;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace PDV.Services
{
    public class PdvFeatureService : IPdvFeatureService
    {
        private readonly PdvContext _context;
        private readonly IReadOnlyList<PdvScreenDefinition> _telas;

        public PdvFeatureService(PdvContext context)
        {
            _context = context;
            _telas = CriarCatalogo()
                .Concat(CriarCatalogoObrigatorioCadastros())
                .DistinctBy(t => t.Key, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public IReadOnlyList<PdvScreenDefinition> ListarTelas(string modulo)
        {
            return _telas
                .Where(t => t.Modulo.Equals(modulo, StringComparison.OrdinalIgnoreCase))
                .OrderBy(t => t.Titulo)
                .ToList();
        }

        public PdvScreenDefinition ObterTela(string telaKey)
        {
            return _telas.First(t => t.Key.Equals(telaKey, StringComparison.OrdinalIgnoreCase));
        }

        public PdvFeatureColumnLayout ObterLayoutListagem(string telaKey)
        {
            var tela = ObterTela(telaKey);
            var entityType = ObterEntityType(tela.TableName);
            if (entityType == null)
            {
                return new PdvFeatureColumnLayout
                {
                    MostrarCodigo = true,
                    MostrarNome = true,
                    MostrarDescricao = true,
                    MostrarStatus = true,
                    MostrarValor = true,
                    MostrarData = true
                };
            }

            var properties = entityType.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return new PdvFeatureColumnLayout
            {
                MostrarCodigo = TemPropriedade(properties, "Codigo", "CodigoInterno", "Gtin", "NumeroDocumento", "CpfCnpj"),
                MostrarNome = true,
                MostrarDescricao = TemPropriedade(properties, "Descricao", "DescricaoPdv", "Historico", "Observacao", "FormaPagamento", "Ingredientes", "InfoAlergico"),
                MostrarStatus = TemPropriedade(properties, "Status", "Situacao", "StatusMovimento", "StatusPagamento", "StatusRecebimento", "Cancelado"),
                MostrarValor = TemPropriedadeNumerica(properties, "Valor", "ValorPendente", "ValorTotal", "ValorFinal", "ValorAPagar", "ValorAReceber", "ValorVenda", "PlanoValor", "Preco", "PrecoVenda", "PrecoCusto"),
                MostrarData = TemPropriedadeData(properties, "Data", "DataCadastro", "DataEmissao", "DataVencimento", "CriadoEm", "AtualizadoEm")
            };
        }

        public IReadOnlyList<PdvFeatureRow> CarregarLinhas(string telaKey)
        {
            var tela = ObterTela(telaKey);
            var entityType = ObterEntityType(tela.TableName);
            if (entityType == null)
                return CriarLinhasOperacionais(tela);

            var set = ObterDbSet(entityType.ClrType);
            return set.Cast<object>()
                .Take(500)
                .Select(entity => MapRow(tela, entity))
                .ToList();
        }

        public PdvFeatureEditModel CriarModeloEdicao(string telaKey, int? id = null)
        {
            var tela = ObterTela(telaKey);
            var entityType = ObterEntityType(tela.TableName)
                ?? throw new InvalidOperationException($"A tela {tela.Titulo} nao possui tabela integrada.");

            var entity = id.HasValue
                ? _context.Find(entityType.ClrType, id.Value)
                : Activator.CreateInstance(entityType.ClrType);

            if (entity == null)
                throw new InvalidOperationException("Registro nao encontrado.");

            var model = new PdvFeatureEditModel
            {
                TelaKey = tela.Key,
                Titulo = id.HasValue ? $"Editar - {tela.Titulo}" : $"Novo - {tela.Titulo}",
                Id = id
            };

            foreach (var property in ListarPropriedadesEditaveis(entityType))
            {
                model.Campos.Add(new PdvFeatureEditField
                {
                    PropertyName = property.Name,
                    Label = CriarRotulo(property.Name),
                    ValorTexto = FormatarValor(property.GetValue(entity)),
                    Tipo = ObterTipoCampo(property),
                    Obrigatorio = EhCampoObrigatorio(property)
                });
            }

            return model;
        }

        public int SalvarRegistro(string telaKey, int? id, IReadOnlyDictionary<string, string?> valores)
        {
            var tela = ObterTela(telaKey);
            var entityType = ObterEntityType(tela.TableName)
                ?? throw new InvalidOperationException($"A tela {tela.Titulo} nao possui tabela integrada.");

            var entity = id.HasValue
                ? _context.Find(entityType.ClrType, id.Value)
                : Activator.CreateInstance(entityType.ClrType);

            if (entity == null)
                throw new InvalidOperationException("Registro nao encontrado.");

            foreach (var property in ListarPropriedadesEditaveis(entityType))
            {
                if (!valores.TryGetValue(property.Name, out var valorTexto))
                    continue;

                if (EhCampoObrigatorio(property) && string.IsNullOrWhiteSpace(valorTexto))
                    throw new InvalidOperationException($"O campo {CriarRotulo(property.Name)} e obrigatorio.");

                AplicarValorConvertido(entity, property, valorTexto);
            }

            if (!id.HasValue)
                _context.Add(entity);
            else
                _context.Update(entity);

            AplicarDatas(entity);
            _context.SaveChanges();

            return LerInt(entity, "Id") ?? id ?? 0;
        }

        public void CriarRegistro(string telaKey)
        {
            var model = CriarModeloEdicao(telaKey);
            PreencherValoresPadrao(model);
            SalvarRegistro(telaKey, null, model.ToDictionary());
        }

        public void AtualizarRegistro(string telaKey, int id)
        {
            var model = CriarModeloEdicao(telaKey, id);
            PreencherValoresEdicao(model);
            SalvarRegistro(telaKey, id, model.ToDictionary());
        }

        public void ExcluirRegistro(string telaKey, int id)
        {
            var tela = ObterTela(telaKey);
            var entityType = ObterEntityType(tela.TableName);
            if (entityType == null)
                return;

            var entity = _context.Find(entityType.ClrType, id);
            if (entity == null)
                return;

            _context.Remove(entity);
            _context.SaveChanges();
        }

        private IEntityType? ObterEntityType(string? tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return null;

            return _context.Model.GetEntityTypes()
                .FirstOrDefault(e => string.Equals(e.GetTableName(), tableName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable ObterDbSet(Type clrType)
        {
            var method = typeof(DbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethodDefinition && m.GetParameters().Length == 0);

            return (IEnumerable)(method.MakeGenericMethod(clrType).Invoke(_context, null)
                ?? throw new InvalidOperationException("DbSet nao encontrado."));
        }

        private static PdvFeatureRow MapRow(PdvScreenDefinition tela, object entity)
        {
            return new PdvFeatureRow
            {
                Id = LerInt(entity, "Id"),
                Codigo = LerTexto(entity, "Codigo", "CodigoInterno", "Gtin", "NumeroDocumento", "CpfCnpj"),
                Nome = ValorOuPadrao(
                    LerTexto(entity, "Nome", "RazaoSocial", "NomeFantasia", "Fantasia", "Descricao", "DescricaoPdv", "Pergunta", "Resposta", "ModoPreparo", "Historico", "Observacao"),
                    $"{tela.Titulo} #{LerInt(entity, "Id")}"),
                Descricao = LerTexto(entity, "Descricao", "DescricaoPdv", "Historico", "Observacao", "FormaPagamento", "Ingredientes", "InfoAlergico"),
                Status = LerTexto(entity, "Status", "Situacao", "StatusMovimento", "StatusPagamento", "StatusRecebimento", "Cancelado"),
                Valor = LerDecimal(entity, "Valor", "ValorPendente", "ValorTotal", "ValorFinal", "ValorAPagar", "ValorAReceber", "ValorVenda", "PlanoValor"),
                Data = LerData(entity, "DataCadastro", "DataPedido", "DataVenda", "DataLancamento", "DataAbertura", "DataFechamento")
            };
        }

        private static IReadOnlyList<PdvFeatureRow> CriarLinhasOperacionais(PdvScreenDefinition tela)
        {
            return new[]
            {
                new PdvFeatureRow
                {
                    Codigo = tela.Key,
                    Nome = tela.Titulo,
                    Descricao = tela.Descricao,
                    Status = tela.Status,
                    Data = DateTime.Today
                }
            };
        }

        private static void AplicarDatas(object entity)
        {
            foreach (var name in new[] { "DataCadastro", "DataPedido", "DataLancamento", "DataAbertura", "DataVenda" })
                AplicarValor(entity, name, DateTime.Today);

            AplicarValor(entity, "HoraAbertura", DateTime.Now.ToString("HH:mm:ss"));
            AplicarValor(entity, "HoraVenda", DateTime.Now.ToString("HH:mm:ss"));
        }

        private static void AplicarValor(object entity, string propertyName, object value)
        {
            var property = entity.GetType().GetProperty(propertyName);
            if (property == null || !property.CanWrite)
                return;

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (targetType == typeof(string))
                property.SetValue(entity, value.ToString());
            else if (targetType == typeof(DateTime) && value is DateTime date)
                property.SetValue(entity, date);
            else if (targetType == typeof(double))
                property.SetValue(entity, 0d);
            else if (targetType == typeof(int))
                property.SetValue(entity, 0);
        }

        private static void AplicarValorConvertido(object entity, PropertyInfo property, string? valorTexto)
        {
            if (!property.CanWrite)
                return;

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (string.IsNullOrWhiteSpace(valorTexto))
            {
                property.SetValue(entity, Nullable.GetUnderlyingType(property.PropertyType) != null || targetType == typeof(string)
                    ? null
                    : Activator.CreateInstance(targetType));
                return;
            }

            object value;
            if (targetType == typeof(string))
                value = valorTexto.Trim();
            else if (targetType == typeof(int))
                value = int.Parse(valorTexto, NumberStyles.Integer, CultureInfo.CurrentCulture);
            else if (targetType == typeof(double))
                value = double.Parse(valorTexto, NumberStyles.Number, CultureInfo.CurrentCulture);
            else if (targetType == typeof(decimal))
                value = decimal.Parse(valorTexto, NumberStyles.Number, CultureInfo.CurrentCulture);
            else if (targetType == typeof(DateTime))
                value = DateTime.Parse(valorTexto, CultureInfo.CurrentCulture);
            else if (targetType == typeof(bool))
                value = valorTexto.Equals("S", StringComparison.OrdinalIgnoreCase)
                    || valorTexto.Equals("true", StringComparison.OrdinalIgnoreCase)
                    || valorTexto.Equals("sim", StringComparison.OrdinalIgnoreCase);
            else
                value = Convert.ChangeType(valorTexto, targetType, CultureInfo.CurrentCulture);

            property.SetValue(entity, value);
        }

        private static IReadOnlyList<PropertyInfo> ListarPropriedadesEditaveis(IEntityType entityType)
        {
            var scalarNames = entityType.GetProperties()
                .Where(p => !p.IsPrimaryKey())
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return entityType.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => scalarNames.Contains(p.Name))
                .Where(p => !Attribute.IsDefined(p, typeof(NotMappedAttribute)))
                .Where(p => EhTipoEditavel(p.PropertyType))
                .OrderBy(p => OrdemCampo(p.Name))
                .ThenBy(p => p.Name)
                .ToList();
        }

        private static bool EhTipoEditavel(Type type)
        {
            var targetType = Nullable.GetUnderlyingType(type) ?? type;
            return targetType == typeof(string)
                || targetType == typeof(int)
                || targetType == typeof(double)
                || targetType == typeof(decimal)
                || targetType == typeof(DateTime)
                || targetType == typeof(bool);
        }

        private static int OrdemCampo(string nome)
        {
            var preferenciais = new[]
            {
                "Codigo", "CodigoInterno", "Gtin", "Ncm", "Cfop", "Nome", "RazaoSocial", "NomeFantasia",
                "Descricao", "DescricaoPdv", "Pergunta", "Resposta", "Status", "Situacao", "Valor", "ValorPendente"
            };

            var index = Array.FindIndex(preferenciais, p => p.Equals(nome, StringComparison.OrdinalIgnoreCase));
            return index >= 0 ? index : 100;
        }

        private static string ObterTipoCampo(PropertyInfo property)
        {
            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (targetType == typeof(DateTime))
                return "Data";
            if (targetType == typeof(int) || targetType == typeof(double) || targetType == typeof(decimal))
                return "Numero";
            if (targetType == typeof(bool))
                return "Booleano";
            return "Texto";
        }

        private static bool EhCampoObrigatorio(PropertyInfo property)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) == null
                && property.PropertyType != typeof(string)
                && !property.Name.StartsWith("Id", StringComparison.OrdinalIgnoreCase);
        }

        private static string CriarRotulo(string propertyName)
        {
            return string.Concat(propertyName.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
        }

        private static string? FormatarValor(object? value)
        {
            return value switch
            {
                null => null,
                DateTime date => date.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture),
                double number => number.ToString("N2", CultureInfo.CurrentCulture),
                decimal number => number.ToString("N2", CultureInfo.CurrentCulture),
                bool boolean => boolean ? "S" : "N",
                _ => value.ToString()
            };
        }

        private static void PreencherValoresPadrao(PdvFeatureEditModel model)
        {
            foreach (var campo in model.Campos)
            {
                if (!string.IsNullOrWhiteSpace(campo.ValorTexto))
                    continue;

                campo.ValorTexto = campo.Tipo switch
                {
                    "Numero" => campo.PropertyName.StartsWith("Id", StringComparison.OrdinalIgnoreCase) ? "1" : "1",
                    "Data" => DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture),
                    "Booleano" => "N",
                    _ => ValorTextoPadrao(model.Titulo, campo.PropertyName)
                };
            }
        }

        private static void PreencherValoresEdicao(PdvFeatureEditModel model)
        {
            foreach (var campo in model.Campos.Where(c => c.Tipo == "Texto").Take(2))
                campo.ValorTexto = $"{ValorOuPadrao(campo.ValorTexto, campo.Label)} editado";

            var primeiroNumero = model.Campos.FirstOrDefault(c => c.Tipo == "Numero" && !c.PropertyName.StartsWith("Id", StringComparison.OrdinalIgnoreCase));
            if (primeiroNumero != null)
                primeiroNumero.ValorTexto = "2";
        }

        private static string ValorTextoPadrao(string titulo, string propertyName)
        {
            if (propertyName.Contains("Email", StringComparison.OrdinalIgnoreCase))
                return "teste@local.com";
            if (propertyName.Contains("Cnpj", StringComparison.OrdinalIgnoreCase))
                return "11222333000181";
            if (propertyName.Contains("Cpf", StringComparison.OrdinalIgnoreCase))
                return "12345678910";
            if (propertyName.Contains("Uf", StringComparison.OrdinalIgnoreCase))
                return "SP";
            if (propertyName.Contains("Status", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("Situacao", StringComparison.OrdinalIgnoreCase))
                return "Ativo";

            return $"{titulo.Replace("Novo - ", string.Empty).Replace("Editar - ", string.Empty)} teste";
        }

        private static int? LerInt(object entity, string propertyName)
        {
            var value = entity.GetType().GetProperty(propertyName)?.GetValue(entity);
            return value switch
            {
                int intValue => intValue,
                _ => null
            };
        }

        private static string LerTexto(object entity, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var value = entity.GetType().GetProperty(propertyName)?.GetValue(entity)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            return string.Empty;
        }

        private static string ValorOuPadrao(params string[] valores)
        {
            foreach (var valor in valores)
            {
                if (!string.IsNullOrWhiteSpace(valor) && !valor.EndsWith("#", StringComparison.OrdinalIgnoreCase))
                    return valor;
            }

            return string.Empty;
        }

        private static bool TemPropriedade(IEnumerable<PropertyInfo> properties, params string[] nomes)
        {
            return properties.Any(p => nomes.Any(n => p.Name.Equals(n, StringComparison.OrdinalIgnoreCase)));
        }

        private static bool TemPropriedadeNumerica(IEnumerable<PropertyInfo> properties, params string[] nomes)
        {
            return properties.Any(p =>
                nomes.Any(n => p.Name.Equals(n, StringComparison.OrdinalIgnoreCase)) &&
                EhTipoNumerico(Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType));
        }

        private static bool TemPropriedadeData(IEnumerable<PropertyInfo> properties, params string[] nomes)
        {
            return properties.Any(p =>
                nomes.Any(n => p.Name.Equals(n, StringComparison.OrdinalIgnoreCase)) &&
                (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType) == typeof(DateTime));
        }

        private static bool EhTipoNumerico(Type type)
        {
            return type == typeof(decimal)
                || type == typeof(double)
                || type == typeof(float)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(short);
        }

        private static decimal LerDecimal(object entity, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var value = entity.GetType().GetProperty(propertyName)?.GetValue(entity);
                if (value is double doubleValue)
                    return Convert.ToDecimal(doubleValue);
                if (value is decimal decimalValue)
                    return decimalValue;
                if (value is int intValue)
                    return intValue;
            }

            return 0;
        }

        private static DateTime? LerData(object entity, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var value = entity.GetType().GetProperty(propertyName)?.GetValue(entity);
                if (value is DateTime dateValue)
                    return dateValue;
            }

            return null;
        }

        private static IReadOnlyList<PdvScreenDefinition> CriarCatalogo()
        {
            return new List<PdvScreenDefinition>
            {
                new("cliente", "Cadastros", "Cliente / Consumidor", "Cadastro de pessoas clientes e consumidores.", "CLIENTE", "Integrada"),
                new("fornecedor", "Cadastros", "Fornecedor", "Cadastro de fornecedores.", "FORNECEDOR", "Integrada"),
                new("colaborador", "Cadastros", "Colaborador", "Cadastro de colaboradores e operadores administrativos.", "COLABORADOR", "Integrada"),
                new("empresa", "Cadastros", "Empresa", "Cadastro da empresa local.", "EMPRESA", "Integrada"),
                new("produto", "Cadastros", "Produto", "Cadastro principal de produtos.", "PRODUTO", "Integrada"),
                new("produto_unidade", "Cadastros", "Unidade de Produto", "Unidades e fracionamento de produto.", "PRODUTO_UNIDADE", "Integrada"),
                new("forma_pagamento", "Cadastros", "Forma de Pagamento", "Tipos e regras de pagamento do PDV.", "PDV_TIPO_PAGAMENTO", "Integrada"),

                new("cliente_fiado", "CadastrosPlus", "Cliente Fiado", "Controle de limite e fiado por cliente.", "CLIENTE_FIADO", "Integrada"),
                new("nfce_plano_pagamento", "CadastrosPlus", "Plano de Pagamento NFC-e", "Planos, pagamentos e transacoes NFC-e.", "NFCE_PLANO_PAGAMENTO", "Integrada"),
                new("taxa_entrega", "CadastrosPlus", "Taxa de Entrega", "Tabela de taxas de entrega.", "TAXA_ENTREGA", "Integrada"),
                new("produto_grupo", "CadastrosPlus", "Grupo de Produto", "Cadastro complementar de agrupamento de produtos.", "PRODUTO_GRUPO", "Integrada"),
                new("produto_subgrupo", "CadastrosPlus", "Subgrupo de Produto", "Cadastro complementar de subgrupos de produtos.", "PRODUTO_SUBGRUPO", "Integrada"),
                new("produto_tipo", "CadastrosPlus", "Tipo de Produto", "Classificacao de produtos.", "PRODUTO_TIPO", "Integrada"),
                new("produto_ficha_tecnica", "CadastrosPlus", "Ficha Técnica", "Componentes e composição do produto.", "PRODUTO_FICHA_TECNICA", "Integrada"),
                new("cardapio", "CadastrosPlus", "Cardápio", "Cadastro de produto para operação food.", "CARDAPIO", "Integrada"),

                new("movimento_lista", "MovimentoPlus", "Movimentos", "Consulta de movimentos de caixa.", "PDV_MOVIMENTO", "Integrada"),
                new("movimento_inicia", "MovimentoPlus", "Iniciar Movimento", "Abertura de caixa local.", "PDV_MOVIMENTO", "Integrada"),
                new("movimento_encerra", "MovimentoPlus", "Encerrar Movimento", "Fechamento e conferência de caixa.", "PDV_MOVIMENTO", "Integrada"),

                new("pdv_identifica_cliente", "PdvPlus", "Identificar Cliente", "Associa consumidor ao atendimento.", "CLIENTE", "Integrada"),
                new("pdv_informa_valor", "PdvPlus", "Informar Valor", "Entrada rápida de valores auxiliares.", null, "Operacional"),
                new("pdv_produto_detalhe", "PdvPlus", "Detalhe do Produto", "Consulta detalhada do item vendido.", "PRODUTO", "Integrada"),
                new("pdv_pagamento", "PdvPlus", "Efetuar Pagamento", "Fluxo de pagamento da venda.", "PDV_TOTAL_TIPO_PAGAMENTO", "Integrada"),
                new("pdv_parcelamento_receitas", "PdvPlus", "Parcelamento de Receitas", "Controle de parcelas a receber.", "CONTAS_RECEBER", "Integrada"),

                new("nfce_contrata", "Fiscal", "Contratar NFC-e", "Dados de contratação/habilitação fiscal.", "NFE_CONFIGURACAO", "Integrada"),
                new("nfce_inutiliza", "Fiscal", "Inutilizar Número", "Controle de inutilização numérica NFC-e.", "NFE_NUMERO_INUTILIZADO", "Integrada"),
                new("nfe_cabecalho", "Fiscal", "NFC-e Cabeçalho", "Consulta e manutenção de documentos fiscais.", "NFE_CABECALHO", "Integrada"),
                new("nfe_detalhe", "Fiscal", "NFC-e Detalhes", "Itens do documento fiscal.", "NFE_DETALHE", "Integrada"),
                new("nfe_devolucao", "Fiscal", "Devolução NFC-e", "Fluxo de devolução fiscal.", "NFE_CABECALHO", "Integrada"),

                new("tribut_grupo", "Tributacao", "Grupo Tributário", "Cadastro de grupos tributários.", "TRIBUT_GRUPO_TRIBUTARIO", "Integrada"),
                new("tribut_operacao", "Tributacao", "Operação Fiscal", "Operações fiscais padrão.", "TRIBUT_OPERACAO_FISCAL", "Integrada"),
                new("tribut_config_of_gt", "Tributacao", "Configuração OF/GT", "Vínculo de operação fiscal e grupo tributário.", "TRIBUT_CONFIGURA_OF_GT", "Integrada"),
                new("tribut_icms_uf", "Tributacao", "ICMS UF", "Alíquotas e regras de ICMS por UF.", "TRIBUT_ICMS_UF", "Integrada"),
                new("tribut_pis", "Tributacao", "PIS", "Configurações de PIS.", "TRIBUT_PIS", "Integrada"),
                new("tribut_cofins", "Tributacao", "COFINS", "Configurações de COFINS.", "TRIBUT_COFINS", "Integrada"),

                new("food_mesa", "Food", "Mesas", "Operação e cadastro de mesas.", "MESA", "Integrada"),
                new("food_comanda", "Food", "Comandas", "Controle de comandas.", "COMANDA", "Integrada"),
                new("food_comanda_detalhe", "Food", "Itens da Comanda", "Detalhamento de itens da comanda.", "COMANDA_DETALHE", "Integrada"),
                new("food_observacao", "Food", "Observações Padrão", "Observações de preparo/atendimento.", "COMANDA_OBSERVACAO_PADRAO", "Integrada"),
                new("food_cozinha", "Food", "Cozinha", "Cadastro e controle de cozinha.", "COZINHA", "Integrada"),
                new("food_reserva", "Food", "Reservas", "Controle de reservas.", "RESERVA", "Integrada"),
                new("food_cardapio_digital", "Food", "Cardápio Digital", "Consulta operacional do cardápio.", "CARDAPIO", "Integrada"),

                new("delivery", "Delivery", "Delivery", "Pedidos e entregas.", "DELIVERY", "Integrada"),
                new("delivery_taxa", "Delivery", "Taxas de Entrega", "Tabela de taxas de entrega.", "TAXA_ENTREGA", "Integrada"),
                new("delivery_acerto", "Delivery", "Acerto de Delivery", "Acerto financeiro de entregas.", "DELIVERY_ACERTO", "Integrada"),

                new("rel_recibo_a4", "Relatorios", "Recibo A4", "Emissão de recibo em formato A4.", null, "Operacional"),
                new("rel_recibo_80", "Relatorios", "Recibo 80mm", "Emissão de recibo em bobina 80mm.", null, "Operacional"),
                new("rel_recibo_57", "Relatorios", "Recibo 57mm", "Emissão de recibo em bobina 57mm.", null, "Operacional"),
                new("rel_movimento", "Relatorios", "Relatório de Movimento", "Fechamento e conferência de movimento.", "PDV_MOVIMENTO", "Integrada"),
                new("rel_comanda", "Relatorios", "Relatório de Comanda", "Impressão/consulta de comanda.", "COMANDA", "Integrada"),

                new("config_geral", "ConfiguracoesPlus", "Configuração Geral", "Parâmetros gerais do PDV.", "PDV_CONFIGURACAO", "Integrada"),
                new("config_nfce", "ConfiguracoesPlus", "Configuração NFC-e", "Parâmetros fiscais e certificados.", "NFE_CONFIGURACAO", "Integrada"),
                new("config_cadastro", "ConfiguracoesPlus", "Configuração de Cadastro", "Parâmetros de cadastros e validações.", "PDV_CONFIGURACAO", "Integrada"),
            };
        }
        public static IReadOnlyList<PdvScreenDefinition> CriarCatalogoObrigatorioCadastros()
        {
            return new List<PdvScreenDefinition>
            {
                new("cliente", "Cadastros", "Cliente / Consumidor", "Cadastro de pessoas clientes e consumidores.", "CLIENTE", "Integrada"),
                new("fornecedor", "Cadastros", "Fornecedor", "Cadastro de fornecedores.", "FORNECEDOR", "Integrada"),
                new("colaborador", "Cadastros", "Colaborador", "Cadastro de colaboradores.", "COLABORADOR", "Integrada"),
                new("empresa", "Cadastros", "Empresa", "Cadastro da empresa.", "EMPRESA", "Integrada"),
                new("produto", "Cadastros", "Produto", "Cadastro principal de produtos.", "PRODUTO", "Integrada"),
                new("produto_unidade", "Cadastros", "Unidade de Produto", "Unidades e fracionamento de produto.", "PRODUTO_UNIDADE", "Integrada"),
                new("forma_pagamento", "Cadastros", "Forma de Pagamento", "Formas e tipos de pagamento do PDV.", "PDV_TIPO_PAGAMENTO", "Integrada"),
                new("cliente_fiado", "CadastrosPlus", "Cliente Fiado", "Controle de limite e fiado por cliente.", "CLIENTE_FIADO", "Integrada"),
                new("nfce_plano_pagamento", "CadastrosPlus", "Plano de Pagamento NFC-e", "Planos, pagamentos e transacoes NFC-e.", "NFCE_PLANO_PAGAMENTO", "Integrada"),
                new("taxa_entrega", "CadastrosPlus", "Taxa de Entrega", "Tabela de taxas de entrega.", "TAXA_ENTREGA", "Integrada"),
                new("cardapio", "CadastrosPlus", "Cardapio", "Cadastro de produto para operacao food.", "CARDAPIO", "Integrada"),
                new("cardapio_pergunta", "CadastrosPlus", "Perguntas do Cardapio", "Perguntas padrao do cardapio.", "CARDAPIO_PERGUNTA_PADRAO", "Integrada"),
                new("cardapio_resposta", "CadastrosPlus", "Respostas do Cardapio", "Respostas padrao do cardapio.", "CARDAPIO_RESPOSTA_PADRAO", "Integrada"),
                new("cozinha", "CadastrosPlus", "Cozinha", "Cadastro de cozinhas e setores de preparo.", "COZINHA", "Integrada"),
                new("mesa", "CadastrosPlus", "Mesa", "Cadastro de mesas.", "MESA", "Integrada"),
                new("produto_grupo", "CadastrosPlus", "Produto Grupo", "Cadastro complementar de agrupamento de produtos.", "PRODUTO_GRUPO", "Integrada"),
                new("produto_subgrupo", "CadastrosPlus", "Produto Subgrupo", "Cadastro complementar de subgrupos de produtos.", "PRODUTO_SUBGRUPO", "Integrada"),
                new("produto_tipo", "CadastrosPlus", "Produto Tipo", "Classificacao de produtos.", "PRODUTO_TIPO", "Integrada"),
                new("produto_imagem", "CadastrosPlus", "Produto Imagem", "Imagens vinculadas ao produto.", "PRODUTO_IMAGEM", "Integrada"),
                new("produto_promocao", "CadastrosPlus", "Produto Promocao", "Promocoes e precificacao promocional.", "PRODUTO_PROMOCAO", "Integrada"),
                new("produto_ficha_tecnica", "CadastrosPlus", "Produto Ficha Tecnica", "Componentes e composicao do produto.", "PRODUTO_FICHA_TECNICA", "Integrada"),
                new("contador", "CadastrosPlus", "Contador", "Dados do contador responsavel.", "CONTADOR", "Integrada"),
                new("pdv_configuracao", "ConfiguracoesPlus", "Configuracao PDV", "Parametros gerais do PDV.", "PDV_CONFIGURACAO", "Integrada"),
                new("pdv_configuracao_balanca", "ConfiguracoesPlus", "Configuracao de Balanca", "Configuracao da balanca integrada.", "PDV_CONFIGURACAO_BALANCA", "Integrada"),
                new("pdv_configuracao_leitor_serial", "ConfiguracoesPlus", "Configuracao de Leitor Serial", "Configuracao do leitor serial.", "PDV_CONFIGURACAO_LEITOR_SERIAL", "Integrada"),
                new("pdv_operador", "ConfiguracoesPlus", "Operador PDV", "Operadores e permissoes do PDV.", "PDV_OPERADOR", "Integrada"),
                new("pdv_tipo_pagamento", "ConfiguracoesPlus", "Tipo de Pagamento PDV", "Tipos de pagamento do PDV.", "PDV_TIPO_PAGAMENTO", "Integrada"),
                new("cfop", "Fiscal", "CFOP", "Cadastro de CFOP.", "CFOP", "Integrada"),
                new("ecf_aliquotas", "Fiscal", "Aliquota ECF", "Aliquotas usadas por ECF.", "ECF_ALIQUOTAS", "Integrada"),
                new("ecf_impressora", "Fiscal", "Impressora ECF", "Cadastro de impressoras ECF.", "ECF_IMPRESSORA", "Integrada"),
                new("nfe_configuracao", "Fiscal", "Configuracao NFe/NFC-e", "Parametros fiscais e certificados.", "NFE_CONFIGURACAO", "Integrada"),
                new("nfe_responsavel_tecnico", "Fiscal", "Responsavel Tecnico NFe", "Responsavel tecnico informado na NFe.", "NFE_RESPONSAVEL_TECNICO", "Integrada"),
                new("tribut_grupo", "Tributacao", "Grupo Tributario", "Cadastro de grupos tributarios.", "TRIBUT_GRUPO_TRIBUTARIO", "Integrada"),
                new("tribut_operacao", "Tributacao", "Operacao Fiscal", "Operacoes fiscais padrao.", "TRIBUT_OPERACAO_FISCAL", "Integrada"),
                new("tribut_icms", "Tributacao", "Tributacao ICMS", "Regras customizadas de tributacao ICMS.", "TRIBUT_ICMS_CUSTOM_CAB", "Integrada"),
                new("tribut_pis", "Tributacao", "Tributacao PIS", "Configuracoes de PIS.", "TRIBUT_PIS", "Integrada"),
                new("tribut_cofins", "Tributacao", "Tributacao COFINS", "Configuracoes de COFINS.", "TRIBUT_COFINS", "Integrada"),
                new("tribut_ipi", "Tributacao", "Tributacao IPI", "Configuracoes de IPI.", "TRIBUT_IPI", "Integrada"),
                new("tribut_iss", "Tributacao", "Tributacao ISS", "Configuracoes de ISS.", "TRIBUT_ISS", "Integrada"),
                new("ibpt", "Tributacao", "IBPT", "Tabela de carga tributaria IBPT.", "IBPT", "Integrada"),
            };
        }
    }
}
