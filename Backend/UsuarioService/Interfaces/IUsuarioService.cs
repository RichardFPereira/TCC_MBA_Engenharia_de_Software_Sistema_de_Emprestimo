using Backend.UsuarioService.DTOs;

namespace Backend.UsuarioService.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDTO> CadastrarAsync(CreateUsuarioDTO dto);
        Task<UsuarioResponseDTO> LoginAsync(LoginDTO loginDto);
    }
}