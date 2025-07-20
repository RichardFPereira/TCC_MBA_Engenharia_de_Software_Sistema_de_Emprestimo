using Backend.UsuarioService.DTOs;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDTO> CadastrarAsync(CreateUsuarioDTO dto);
    Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
}