using Backend.UsuarioService.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface IEmprestimoRepository
{
    Task AddEmprestimoAsync(Emprestimo emprestimo, List<Parcela> parcelas);
    Task<List<Emprestimo>> GetEmprestimosByUsuarioIdAsync(int usuarioId);
    Task<Emprestimo?> GetEmprestimoByIdAsync(int id);
    Task<Usuario?> GetUsuarioByIdAsync(int usuarioId);
    Task<bool> HasEmprestimoAtivoAsync(int usuarioId);
    Task UpdateEmprestimoAsync(Emprestimo emprestimo);
    Task<Parcela?> GetParcelaByIdAsync(int parcelaId);
    Task UpdateParcelaAsync(Parcela parcela);
    Task<bool> AllParcelasPagasAsync(int emprestimoId);
}