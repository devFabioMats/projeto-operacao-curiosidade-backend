using Microsoft.EntityFrameworkCore;
using OperacaoCuriosidadeAPI.Models;

namespace OperacaoCuriosidadeAPI.Context
{
    public class OperacaoCuriosidadeContext : DbContext   // esse context faz a ligação com o banco de dados
    {
        // responsavel por passar a configurção de banco de dados da DbContext
        public OperacaoCuriosidadeContext(DbContextOptions<OperacaoCuriosidadeContext> options) : base(options)
        {
        }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
