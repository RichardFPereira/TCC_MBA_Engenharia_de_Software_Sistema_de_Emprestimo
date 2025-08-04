using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Models.Entities
{
    public class Configuracao
    {
        public int Id { get; set; }
        public decimal TaxaJuros { get; set; }
        public int MinParcelas { get; set; }
        public int MaxParcelas { get; set; }
        public decimal PercentualReserva { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}