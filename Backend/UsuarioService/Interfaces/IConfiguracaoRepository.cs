using Backend.UsuarioService.Models.Entities;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface IConfiguracaoRepository
{
    Task AddConfiguracaoAsync(Configuracao configuracao);
    Task<Configuracao?> GetConfiguracaoMaisRecenteAsync();
}