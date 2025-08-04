using Backend.UsuarioService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface ILogAcoesService
{
    Task AddLogAcaoAsync(string acao, string detalhes, int administradorId);
    Task<List<LogAcoesResponseDTO>> GetLogsByAdministradorIdAsync(int administradorId);
}