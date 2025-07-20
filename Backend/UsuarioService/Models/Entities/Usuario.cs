using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Backend.UsuarioService.Models.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required]
        [StringLength(11)]
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public decimal Salario { get; set; }
        public decimal Reserva { get; set; }
        public string Role { get; set; } = "Participante";
        public Credencial Credencial { get; set; } = null!;
    }
}