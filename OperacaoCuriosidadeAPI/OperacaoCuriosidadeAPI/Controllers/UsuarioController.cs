using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;
using OperacaoCuriosidadeAPI.Models;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("oc-api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly OperacaoCuriosidadeContext _context;

        public UsuarioController(OperacaoCuriosidadeContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Dados inválidos");

            bool existe = _context.Usuarios.Any(u => u.Nome == usuario.Nome || u.Email == usuario.Email);
            if (existe)
            {
                return BadRequest("Usuário já cadastrado");
            }

            _context.Add(usuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound("Não encontrado");
            }
            return Ok(usuario);
        }

        [HttpGet("ObterPorNome")]
        public IActionResult ObterPorNome(string nome)
        {
            var usuarios = _context.Usuarios.Where(u => u.Nome.Contains(nome)).ToList();
            if (usuarios == null || !usuarios.Any())
            {
                return NotFound("Não encontrado");
            }
            return Ok(usuarios);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var usuarios = _context.Usuarios.ToList();
            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(id);

            if (usuarioBanco == null)
                return NotFound();

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Senha = usuario.Senha;

            _context.Usuarios.Update(usuarioBanco);
            _context.SaveChanges();

            return Ok(usuarioBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var usuarioBanco = _context.Usuarios.Find(id);
            if (usuarioBanco == null)
                return NotFound("Não encontrado");

            _context.Usuarios.Remove(usuarioBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
