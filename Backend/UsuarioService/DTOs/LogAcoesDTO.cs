using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.UsuarioService.DTOs;

public class LogAcoesResponseDTO
{
    public int Id { get; set; }
    public int AdministradorId { get; set; }
    public string NomeAdministrador { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    public string Detalhes { get; set; } = string.Empty;
    public DateTime Data { get; set; }
}