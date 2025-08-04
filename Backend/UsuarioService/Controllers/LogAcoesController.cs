using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class LogAcoesController : ControllerBase
{
    private readonly ILogAcoesService _logAcoesService;

    public LogAcoesController(ILogAcoesService logAcoesService)
    {
        _logAcoesService = logAcoesService;
    }

    [HttpPost]
    public async Task<IActionResult> AddLogAcao([FromQuery] string acao, [FromQuery] string detalhes, [FromQuery] int administradorId)
    {
        await _logAcoesService.AddLogAcaoAsync(acao, detalhes, administradorId);
        return Ok();
    }

    [HttpGet("administrador/{administradorId}")]
    public async Task<IActionResult> GetLogsByAdministradorId(int administradorId)
    {
        var logs = await _logAcoesService.GetLogsByAdministradorIdAsync(administradorId);
        return Ok(logs);
    }
}