using Backend.UsuarioService.Data;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Repositories;

public class LogAcoesRepository : ILogAcoesRepository
{
    private readonly UsuarioDbContext _context;

    public LogAcoesRepository(UsuarioDbContext context)
    {
        _context = context;
    }

    public async Task AddLogAcaoAsync(LogAcoes log)
    {
        await _context.LogAcoes.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LogAcoes>> GetLogsByAdministradorIdAsync(int administradorId)
    {
        return await _context.LogAcoes
            .Include(l => l.Administrador)
            .Where(l => l.AdministradorId == administradorId)
            .ToListAsync();
    }
}