using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmprestimosController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimosController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpPost]
    [Authorize(Roles = "Participante")]
    public async Task<IActionResult> CriarEmprestimo([FromBody] CreateEmprestimoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _emprestimoService.CriarEmprestimoAsync(dto);
        return CreatedAtAction(nameof(GetEmprestimoById), new { id = response.Id }, response);
    }

    [HttpGet("usuario/{usuarioId}")]
    [Authorize(Roles = "Participante")]
    public async Task<IActionResult> ListarEmprestimosPorUsuario(int usuarioId)
    {
        var emprestimos = await _emprestimoService.ListarEmprestimosPorUsuarioAsync(usuarioId);
        return Ok(emprestimos);
    }

    [HttpPut("{id}/autorizar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AutorizarEmprestimo(int id, [FromBody] bool autorizar)
    {
        var response = await _emprestimoService.AutorizarEmprestimoAsync(id, autorizar);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Participante")]
    public async Task<IActionResult> GetEmprestimoById(int id)
    {
        var emprestimo = await _emprestimoService.ListarEmprestimosPorUsuarioAsync(id); // Ajustar lógica se necessário
        if (emprestimo == null || !emprestimo.Any())
            return NotFound();
        return Ok(emprestimo.First());
    }
}