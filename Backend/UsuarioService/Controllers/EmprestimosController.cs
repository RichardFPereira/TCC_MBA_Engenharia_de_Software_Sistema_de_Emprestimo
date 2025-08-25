using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int authenticatedUserId))
            return Unauthorized("Usuário não autenticado corretamente.");

        var response = await _emprestimoService.CriarEmprestimoAsync(authenticatedUserId, dto);
        return CreatedAtAction(nameof(GetEmprestimoById), new { id = response.Id }, response);
    }

    [HttpGet("usuario/{usuarioId}")]
    [Authorize(Roles = "Participante")]
    public async Task<IActionResult> ListarEmprestimosPorUsuario(int usuarioId)
    {
        var emprestimos = await _emprestimoService.ListarEmprestimosPorUsuarioAsync(usuarioId);
        return Ok(emprestimos);
    }

    [HttpGet("pendentes")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetEmprestimosPendentes()
    {
        var pendentes = await _emprestimoService.GetEmprestimosPendentesAsync();
        return Ok(pendentes);
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
        var emprestimo = await _emprestimoService.ListarEmprestimosPorUsuarioAsync(id);
        if (emprestimo == null || !emprestimo.Any())
            return NotFound();
        return Ok(emprestimo.First());
    }

    [HttpPut("{emprestimoId}/parcelas/{parcelaId}/status")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarStatusParcela(int emprestimoId, int parcelaId, [FromBody] UpdateParcelaStatusDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _emprestimoService.AtualizarStatusParcelaAsync(emprestimoId, parcelaId, dto);
        return Ok(response);
    }

    [HttpPut("autorizar-batch")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AutorizarEmprestimosBatch([FromBody] List<AutorizacaoBatchDTO> dtoList)
    {
        await _emprestimoService.ProcessarAutorizacoesBatchAsync(dtoList);
        return Ok();
    }
}