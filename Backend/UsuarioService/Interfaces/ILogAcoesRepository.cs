using Backend.UsuarioService.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface ILogAcoesRepository
{
    Task AddLogAcaoAsync(LogAcoes log);
    Task<List<LogAcoes>> GetLogsByAdministradorIdAsync(int administradorId);
}