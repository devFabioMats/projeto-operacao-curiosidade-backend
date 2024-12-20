using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;
using OperacaoCuriosidadeAPI.Models;
using OperacaoCuriosidadeAPI.Services;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("oc-api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly OperacaoCuriosidadeContext _context;

        public TokenController(OperacaoCuriosidadeContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos");
            }
            var token = TokenService.GeradorToken(new Models.Usuario());
            return Ok(token);
        }
    }
}