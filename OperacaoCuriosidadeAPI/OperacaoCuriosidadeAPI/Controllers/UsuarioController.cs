using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;
using OperacaoCuriosidadeAPI.Models;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColaboradorController : ControllerBase
    {
        // declarando um campo privado e somente leitura chamado _context do tipo OperacaoCuriosidadeContext.
        private readonly OperacaoCuriosidadeContext _context;

        public ColaboradorController(OperacaoCuriosidadeContext context)
        {
            _context = context;
        }

        // POST:  Colaborador
        [HttpPost]
        public IActionResult Create(Colaborador colaborador)
        {
            _context.Add(colaborador);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = colaborador.Id }, colaborador);
        }

        // GET: Colaborador
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var colaborador = _context.Colaboradores.Find(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            return Ok(colaborador);
        }

        // GET por nome: Colaborador/nome
        [HttpGet("ObterPorNome")]
        public IActionResult ObterPorNome(string nome)
        {
            var colaborador = _context.Colaboradores.Where(c => c.Nome.Contains(nome));
            if (colaborador == null)
            {
                return NotFound();
            }
            return Ok(colaborador);
        }

        // GET todos: Colaborador
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var colaboradores = _context.Colaboradores;
            return Ok(colaboradores);
        }

        // PUT: Colaborador
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Colaborador colaborador)
        {
            var colaboradorBanco = _context.Colaboradores.Find(id);

            if (colaboradorBanco == null)
                return NotFound();

            colaboradorBanco.Status = colaborador.Status;
            colaboradorBanco.Nome = colaborador.Nome;
            colaboradorBanco.Idade = colaborador.Idade;
            colaboradorBanco.Email = colaborador.Email;
            colaboradorBanco.Endereco = colaborador.Endereco;
            colaboradorBanco.Interesses = colaborador.Interesses;
            colaboradorBanco.Sentimentos = colaborador.Sentimentos;
            colaboradorBanco.Valores = colaborador.Valores;

            _context.Colaboradores.Update(colaboradorBanco);
            _context.SaveChanges();

            return Ok(colaboradorBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var colaboradorBanco = _context.Colaboradores.Find(id);
            if (colaboradorBanco == null)
                return NotFound();

            _context.Colaboradores.Remove(colaboradorBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
