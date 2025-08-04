using Backend.UsuarioService.Data;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Repositories;

public class ConfiguracaoRepository : IConfiguracaoRepository
{
    private readonly UsuarioDbContext _context;

    public ConfiguracaoRepository(UsuarioDbContext context)
    {
        _context = context;
    }

    public async Task AddConfiguracaoAsync(Configuracao configuracao)
    {
        await _context.Configuracoes.AddAsync(configuracao);
        await _context.SaveChangesAsync();
    }

    public async Task<Configuracao?> GetConfiguracaoMaisRecenteAsync()
    {
        return await _context.Configuracoes
            .OrderByDescending(c => c.DataCadastro)
            .FirstOrDefaultAsync();
    }
}