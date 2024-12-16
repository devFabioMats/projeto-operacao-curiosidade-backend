﻿using Microsoft.AspNetCore.Mvc;
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
    }
}
