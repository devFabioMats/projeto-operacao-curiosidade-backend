using System.ComponentModel.DataAnnotations;

namespace OperacaoCuriosidadeAPI.Models
{
    public class Colaborador
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Status é obrigatório.")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Idade é obrigatório.")]
        [Range(18, 100, ErrorMessage = "A idade deve estar entre 18 e 100 anos.")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
        public string? Email { get; set; }

        public string? Endereco { get; set; }

        [MaxLength(150, ErrorMessage = "O campo Interesses não pode exceder 150 caracteres.")]
        public string? Interesses { get; set; }

        [MaxLength(150, ErrorMessage = "O campo Sentimentos não pode exceder 150 caracteres.")]
        public string? Sentimentos { get; set; }

        [MaxLength(150, ErrorMessage = "O campo Valores não pode exceder 150 caracteres.")]
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