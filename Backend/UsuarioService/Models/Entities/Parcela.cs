using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Models.Entities
{
    public class Parcela
    {
        public int Id { get; set; }
        public int EmprestimoId { get; set; }
        public int NumeroParcela { get; set; }
        public decimal ValorParcela { get; set; }
        public DateOnly DataVencimento { get; set; }
        public string Status { get; set; } = "Pendente";
        public DateTime? DataPagamento { get; set; }

        public Emprestimo Emprestimo { get; set; } = null!;
    }
}