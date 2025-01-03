﻿using Microsoft.AspNetCore.Mvc;
using OperacaoCuriosidadeAPI.Context;
using OperacaoCuriosidadeAPI.Models;
using OperacaoCuriosidadeAPI.Services;

namespace OperacaoCuriosidadeAPI.Controllers
{
    [ApiController]
    [Route("oc-api/[controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly OperacaoCuriosidadeContext _context;
        private readonly LogService _logService;

        public ColaboradorController(OperacaoCuriosidadeContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        [HttpPost]
        public IActionResult Create(Colaborador colaborador)
        {
            if (colaborador == null)
            {
                return BadRequest("Dados inválidos");
            }

            bool existe = _context.Colaboradores.Any(c => c.Nome == colaborador.Nome || c.Email == colaborador.Email);

            if (existe)
            {
                return BadRequest("Colaborador já cadastrado");
            }

            _context.Add(colaborador);
            _context.SaveChanges();
            _logService.LogParaArquivo("Cadastrar", $"Administrador {User.Identity.Name} cadastrou o colaborador {colaborador.Nome}.");
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
            var colaboradores = _context.Colaboradores.Where(c => c.Nome.Contains(nome));
            if (colaboradores == null)
            {
                return NotFound("Não encontrado");
            }

            return Ok(colaboradores);
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
            if (colaborador == null)
                return BadRequest("Dados inválidos");

            var colaboradorBanco = _context.Colaboradores.Find(id);

            if (colaboradorBanco == null)
                return NotFound();

            bool existe = _context.Colaboradores.Any(c => (c.Email == colaborador.Email || c.Nome == colaborador.Nome) && c.Id != id);
            if (existe)
                return BadRequest("Outro colaborador já cadastrado com o mesmo nome ou email");

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

            _logService.LogParaArquivo("Alterar Dados", $"Administrador {User.Identity.Name} atualizou o colaborador com ID {id}.");
            return Ok(colaboradorBanco);
        }

        [HttpGet("DownloadLog")]
        public IActionResult DownloadLog()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "30122024.txt");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Arquivo de log não encontrado.");
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", "30122024.txt");
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var colaboradorBanco = _context.Colaboradores.Find(id);
            if (colaboradorBanco == null)
                return NotFound("Não encontrado");

            _context.Colaboradores.Remove(colaboradorBanco);
            _context.SaveChanges();

            _logService.LogParaArquivo("Deletar", $"Administrador {User.Identity.Name} deletou o colaborador com ID {id}.");
            return NoContent();
        }

        [HttpGet("Dashboard")]
        public ActionResult<Dashboard> QuantidadeCadastros()
        {
            var cadastros = _context.Colaboradores.ToList();
            int totalCadastros = cadastros.Count;
            int cadastrosInativos = cadastros.Count(c => c.Status == false);
            int cadastrosPendentes = cadastros.Count(c => c.IsPendente);

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