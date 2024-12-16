namespace OperacaoCuriosidadeAPI.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public string? Interesses { get; set; }
        public string? Sentimentos { get; set; }
        public string? Valores { get; set; }

        public bool IsPendente
        {
            get
            {
                return string.IsNullOrEmpty(Endereco) ||
                       string.IsNullOrEmpty(Interesses) ||
                       string.IsNullOrEmpty(Sentimentos) ||
                       string.IsNullOrEmpty(Valores);
            }
        }
    }
}