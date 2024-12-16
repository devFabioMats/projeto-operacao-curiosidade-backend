namespace OperacaoCuriosidadeAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }

        // Relacionamento com Colaboradores
        public ICollection<Colaborador> Colaboradores { get; set; }
    }

}