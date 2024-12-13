using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColaboradorController : ControllerBase
    {
        // declarando um campo privado e somente leitura chamado _context do tipo OperacaoCuriosidadeContext.
        private readonly OperacaoCuriosidadeContext _context;
    }
}
