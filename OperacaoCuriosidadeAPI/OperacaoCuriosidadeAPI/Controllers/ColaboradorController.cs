using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;
using OperacaoCuriosidadeAPI.Models;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("oc-api/[controller]")]
    public class ColaboradorController : ControllerBase
    {
        // declarando um campo privado e somente leitura chamado _context do tipo OperacaoCuriosidadeContext.
        private readonly OperacaoCuriosidadeContext _context;

        public ColaboradorController(OperacaoCuriosidadeContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Colaborador colaborador)
        {
            if (colaborador == null)
                return BadRequest("Dados inválidos");

            bool existe = _context.Colaboradores.Any(colaborador => colaborador.Nome == colaborador.Nome || colaborador.Email == colaborador.Email);
            if (existe)
            {
                return BadRequest("Colaborador já cadastrado");
            }

            _context.Add(colaborador);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = colaborador.Id }, colaborador);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var colaborador = _context.Colaboradores.Find(id);
            if (colaborador == null)
            {
                return NotFound("Não encontrado");
            }
            return Ok(colaborador);
        }

        [HttpGet("ObterPorNome")]
        public IActionResult ObterPorNome(string nome)
        {
            var colaborador = _context.Colaboradores.Where(colaborador => colaborador.Nome.Contains(nome));
            if (colaborador == null)
            {
                return NotFound("Não encontrado");
            }
            return Ok(colaborador);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var colaboradores = _context.Colaboradores;
            return Ok(colaboradores);
        }

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
                return NotFound("Não encontrado");

            _context.Colaboradores.Remove(colaboradorBanco);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("Dashboard")]
        public ActionResult<Dashboard> QuantidadeCadastros()
        {
            var cadastros = _context.Colaboradores.ToList();
            int totalCadastros = cadastros.Count;
            int cadastrosInativos = cadastros.Count(colaborador => colaborador.Status == false);
            int cadastrosPendentes = cadastros.Count(colaborador => colaborador.IsPendente);

            Dashboard dashboardInfos = new Dashboard
            {
                TotalCadastros = totalCadastros,
                CadastrosInativos = cadastrosInativos,
                CadastrosPendentes = cadastrosPendentes
            };

            return Ok(dashboardInfos);
        }
    }
}