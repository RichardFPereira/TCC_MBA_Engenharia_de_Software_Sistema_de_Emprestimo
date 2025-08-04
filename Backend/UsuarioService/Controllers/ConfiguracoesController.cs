using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class ConfiguracoesController : ControllerBase
{
    private readonly IConfiguracaoService _configuracaoService;

    public ConfiguracoesController(IConfiguracaoService configuracaoService)
    {
        _configuracaoService = configuracaoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarConfiguracao([FromBody] CreateConfiguracaoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _configuracaoService.CriarConfiguracaoAsync(dto);
        return CreatedAtAction(nameof(GetConfiguracaoMaisRecente), null, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetConfiguracaoMaisRecente()
    {
        var response = await _configuracaoService.GetConfiguracaoMaisRecenteAsync();
        return Ok(response);
    }
}