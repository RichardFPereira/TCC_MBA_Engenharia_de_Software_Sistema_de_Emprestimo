using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Models.Entities
{
    public class LogAcoes
    {
        public int Id { get; set; }
        public int AdministradorId { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string Detalhes { get; set; } = string.Empty;
        public DateTime Data { get; set; }

        public Usuario Administrador { get; set; } = null!;
    }
}