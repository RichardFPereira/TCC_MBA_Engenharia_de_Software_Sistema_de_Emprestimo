using Backend.UsuarioService.Data;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Repositories;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly UsuarioDbContext _context;

    public EmprestimoRepository(UsuarioDbContext context)
    {
        _context = context;
    }

    public async Task AddEmprestimoAsync(Emprestimo emprestimo, List<Parcela> parcelas)
    {
        await _context.Emprestimos.AddAsync(emprestimo);
        await _context.SaveChangesAsync();
        foreach (var parcela in parcelas)
        {
            parcela.EmprestimoId = emprestimo.Id;
            await _context.Parcelas.AddAsync(parcela);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<List<Emprestimo>> GetEmprestimosByUsuarioIdAsync(int usuarioId)
    {
        return await _context.Emprestimos
            .Include(e => e.Usuario)
            .Where(e => e.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Emprestimo?> GetEmprestimoByIdAsync(int id)
    {
        return await _context.Emprestimos
            .Include(e => e.Usuario)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int usuarioId)
    {
        return await _context.Usuarios.FindAsync(usuarioId);
    }

    public async Task<bool> HasEmprestimoAtivoAsync(int usuarioId)
    {
        return await _context.Emprestimos
            .AnyAsync(e => e.UsuarioId == usuarioId && (e.Status == "Pendente" || e.Status == "Em Andamento"));
    }

    public async Task UpdateEmprestimoAsync(Emprestimo emprestimo)
    {
        _context.Emprestimos.Update(emprestimo);
        await _context.SaveChangesAsync();
    }

    public async Task<Parcela?> GetParcelaByIdAsync(int parcelaId)
    {
        return await _context.Parcelas.FindAsync(parcelaId);
    }

    public async Task UpdateParcelaAsync(Parcela parcela)
    {
        _context.Parcelas.Update(parcela);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AllParcelasPagasAsync(int emprestimoId)
    {
        return !await _context.Parcelas
            .AnyAsync(p => p.EmprestimoId == emprestimoId && p.Status != "Pago");
    }
}