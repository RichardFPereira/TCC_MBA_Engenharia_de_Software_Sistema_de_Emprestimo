using Backend.UsuarioService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Interfaces;

public interface IEmprestimoService
{
    Task<EmprestimoResponseDTO> CriarEmprestimoAsync(CreateEmprestimoDTO dto);
    Task<List<EmprestimoResponseDTO>> ListarEmprestimosPorUsuarioAsync(int usuarioId);
    Task<EmprestimoResponseDTO> AutorizarEmprestimoAsync(int id, bool autorizar);
}