using System;

namespace PDV.Models
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
    }

    public abstract class PessoaBase : EntityBase
    {
        public string? Nome { get; set; }
        public string? Fantasia { get; set; }
        public string? Email { get; set; }
        public string? Url { get; set; }
        public string? CpfCnpj { get; set; }
        public string? Rg { get; set; }
        public string? OrgaoRg { get; set; }
        public DateTime? DataEmissaoRg { get; set; }
        public string? Sexo { get; set; } // M/F
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? TipoPessoa { get; set; } // F/J
        public DateTime? DataCadastro { get; set; } = DateTime.Now;

        // Endereço
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Contato { get; set; }
    }

    public class Cliente : PessoaBase
    {
        // Fidelidade
        public string? FidelidadeAviso { get; set; }
        public decimal? FidelidadeQuantidade { get; set; }
        public decimal? FidelidadeValor { get; set; }

        // Fiado
        public decimal? ValorTeto { get; set; }
    }

    public class Fornecedor : PessoaBase
    {
    }

    public class Colaborador : EntityBase
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public decimal? ComissaoVista { get; set; }
        public decimal? ComissaoPrazo { get; set; }
        public int? NivelAutorizacao { get; set; }
        public string? EntregadorVeiculo { get; set; }
    }

    public class Empresa : EntityBase
    {
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Cnpj { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? TipoRegime { get; set; }
        public string? Crt { get; set; }
        public DateTime? DataConstituicao { get; set; }
        public string? Tipo { get; set; } // Matriz/Filial
        public string? Email { get; set; }
        public decimal? AliquotaPis { get; set; }
        public decimal? AliquotaCofins { get; set; }

        // Endereço
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Fone { get; set; }
        public string? Contato { get; set; }

        public byte[]? Logotipo { get; set; }
        public bool Registrado { get; set; }
        public string? NaturezaJuridica { get; set; }
        public string? EmailPagamento { get; set; }
        public bool Simei { get; set; }
    }

    public class TipoPagamento : EntityBase
    {
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public bool Tef { get; set; }
        public bool ImprimeVinculado { get; set; }
        public bool PermiteTroco { get; set; }
        public bool GeraParcelas { get; set; }
        public string? CodigoPagamentoNfe { get; set; }
    }

    public class UnidadeProduto : EntityBase
    {
        public string? Sigla { get; set; }
        public string? Descricao { get; set; }
        public bool PodeFracionar { get; set; }
    }

    public class Produto : EntityBase
    {
        public string? Gtin { get; set; }
        public string? CodigoInterno { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? DescricaoPdv { get; set; }
        public string? Unidade { get; set; }
        public decimal? ValorCompra { get; set; }
        public decimal? ValorVenda { get; set; }
        public decimal? Estoque { get; set; }
        public decimal? EstoqueMinimo { get; set; }
        public decimal? EstoqueMaximo { get; set; }

        // Fiscal
        public string? Ncm { get; set; }
        public string? Cst { get; set; }
        public string? Csosn { get; set; }
        public decimal? TaxaIpi { get; set; }
        public decimal? TaxaPis { get; set; }
        public decimal? TaxaCofins { get; set; }
        public decimal? TaxaIcms { get; set; }
    }
}
