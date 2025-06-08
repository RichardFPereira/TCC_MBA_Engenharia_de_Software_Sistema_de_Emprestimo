using Backend.UsuarioService.DTOs;

namespace Backend.UsuarioService.Interfaces
{
    public interface IUsuarioService
    {
        Task<object> CadastrarAsync(CreateUsuarioDTO dto);
        Task<object> LoginAsync(LoginDTO loginDto);
    }
}