using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Models.Entities
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorTotal { get; set; }
        public int NumeroParcelas { get; set; }
        public decimal TaxaJuros { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public string Status { get; set; } = "Pendente";
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}