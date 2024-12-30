using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;
using OperacaoCuriosidadeAPI.Models;
using OperacaoCuriosidadeAPI.Services;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("oc-api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly OperacaoCuriosidadeContext _context;
        private readonly LogService _logService;
        private static readonly Dictionary<string, int> tentativas = new Dictionary<string, int>();
        private static readonly Dictionary<string, DateTime> horaUltimaTentativa = new Dictionary<string, DateTime>();
        private const int TentativasLimite = 3;
        private static readonly TimeSpan DuracaoBloqueio = TimeSpan.FromMinutes(1);

        public UsuarioController(OperacaoCuriosidadeContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!tentativas.ContainsKey(login.Email))
            {
                tentativas[login.Email] = 0;
                horaUltimaTentativa[login.Email] = DateTime.UtcNow;
            }

            if (tentativas[login.Email] >= TentativasLimite)
            {
                if (DateTime.UtcNow - horaUltimaTentativa[login.Email] < DuracaoBloqueio)
                {
                    return StatusCode(429, "Muitas tentativas de login. Tente novamente mais tarde.");
                }
                else
                {
                    tentativas[login.Email] = 0;
                }
            }

            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);

            if (usuario == null)
            {
                tentativas[login.Email]++;
                horaUltimaTentativa[login.Email] = DateTime.UtcNow;
                return Unauthorized("Email ou senha inválidos");
            }

            tentativas[login.Email] = 0;

            var token = TokenService.GeradorToken(usuario);
            return Ok(new { token });
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
            _logService.LogParaArquivo("Cadastrar", $"Administrador {User.Identity.Name} cadastrou administrador {usuario.Nome} criado com sucesso.");
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

            bool existe = _context.Usuarios.Any(u => (u.Email == usuario.Email || u.Nome == usuario.Nome) && u.Id != id);
            if (existe)
                return BadRequest("Outro administrador já cadastrado com o mesmo nome ou email");

            if (usuarioBanco == null)
                return NotFound();

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Senha = usuario.Senha;

            _context.Usuarios.Update(usuarioBanco);
            _context.SaveChanges();

            _logService.LogParaArquivo("Alterar", $"Administrador {User.Identity.Name} atualizou o administrador com ID {id}.");
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

            _logService.LogParaArquivo("Deletar", $"Administrador {User.Identity.Name} deletou o administrador com ID {id}.");
            return NoContent();
        }
    }
}