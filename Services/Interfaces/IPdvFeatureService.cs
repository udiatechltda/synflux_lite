using System.Collections.ObjectModel;

namespace PDV.Services.Interfaces
{
    public interface IPdvFeatureService
    {
        IReadOnlyList<PdvScreenDefinition> ListarTelas(string modulo);
        IReadOnlyList<PdvFeatureRow> CarregarLinhas(string telaKey);
        PdvScreenDefinition ObterTela(string telaKey);
        PdvFeatureColumnLayout ObterLayoutListagem(string telaKey);
        PdvFeatureEditModel CriarModeloEdicao(string telaKey, int? id = null);
        int SalvarRegistro(string telaKey, int? id, IReadOnlyDictionary<string, string?> valores);
        void CriarRegistro(string telaKey);
        void AtualizarRegistro(string telaKey, int id);
        void ExcluirRegistro(string telaKey, int id);
    }

    public record PdvScreenDefinition(
        string Key,
        string Modulo,
        string Titulo,
        string Descricao,
        string? TableName,
        string Status);

    public class PdvFeatureRow
    {
        public int? Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime? Data { get; set; }
    }

    public class PdvFeatureColumnLayout
    {
        public bool MostrarCodigo { get; set; }
        public bool MostrarNome { get; set; } = true;
        public bool MostrarDescricao { get; set; }
        public bool MostrarStatus { get; set; }
        public bool MostrarValor { get; set; }
        public bool MostrarData { get; set; }
    }

    public class PdvFeatureEditModel
    {
        public string TelaKey { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int? Id { get; set; }
        public ObservableCollection<PdvFeatureEditField> Campos { get; } = new();

        public IReadOnlyDictionary<string, string?> ToDictionary()
        {
            return Campos.ToDictionary(c => c.PropertyName, c => c.ValorTexto, StringComparer.OrdinalIgnoreCase);
        }
    }

    public class PdvFeatureEditField
    {
        public string PropertyName { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? ValorTexto { get; set; }
        public string Tipo { get; set; } = "Texto";
        public bool Obrigatorio { get; set; }
    }
}
