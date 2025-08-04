using Backend.UsuarioService.DTOs;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface IConfiguracaoService
{
    Task<ConfiguracaoResponseDTO> CriarConfiguracaoAsync(CreateConfiguracaoDTO dto);
    Task<ConfiguracaoResponseDTO> GetConfiguracaoMaisRecenteAsync();
}