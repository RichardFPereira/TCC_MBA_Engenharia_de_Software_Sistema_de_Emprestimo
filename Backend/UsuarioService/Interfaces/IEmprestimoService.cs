using Backend.UsuarioService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface IEmprestimoService
{
    Task<EmprestimoResponseDTO> CriarEmprestimoAsync(int usuarioId, CreateEmprestimoDTO dto);
    Task<List<EmprestimoResponseDTO>> ListarEmprestimosPorUsuarioAsync(int usuarioId);
    Task<EmprestimoResponseDTO> AutorizarEmprestimoAsync(int id, bool autorizar);
    Task<ParcelaResponseDTO> AtualizarStatusParcelaAsync(int emprestimoId, int parcelaId, UpdateParcelaStatusDTO dto);
    Task<List<object>> GetEmprestimosPendentesAsync();
    Task ProcessarAutorizacoesBatchAsync(List<AutorizacaoBatchDTO> dtoList);
}