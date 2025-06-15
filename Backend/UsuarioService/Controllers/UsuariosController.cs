using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.UsuarioService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost("Cadastro")]
        public async Task<ActionResult<UsuarioResponseDTO>> Cadastrar([FromBody] CreateUsuarioDTO dto)
        {
            try
            {
                var response = await _service.CadastrarAsync(dto);
                return CreatedAtAction(nameof(Cadastrar), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponseDTO>> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var response = _service.LoginAsync(dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}