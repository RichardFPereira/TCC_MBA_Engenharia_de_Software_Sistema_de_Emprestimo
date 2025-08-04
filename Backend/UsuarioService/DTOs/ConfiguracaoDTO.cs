using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Backend.UsuarioService.DTOs;

public class CreateConfiguracaoDTO
{
    [Required(ErrorMessage = "A taxa de juros é obrigatória.")]
    [Range(0, double.MaxValue, ErrorMessage = "A taxa de juros deve ser maior ou igual a zero.")]
    public decimal TaxaJuros { get; set; }

    [Required(ErrorMessage = "O número mínimo de parcelas é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O número mínimo de parcelas deve ser maior que zero.")]
    public int MinParcelas { get; set; }

    [Required(ErrorMessage = "O número máximo de parcelas é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O número máximo de parcelas deve ser maior que zero.")]
    public int MaxParcelas { get; set; }

    [Required(ErrorMessage = "O percentual da reserva é obrigatório.")]
    [Range(0, 1, ErrorMessage = "O percentual da reserva deve estar entre 0 e 1.")]
    public decimal PercentualReserva { get; set; }
}

public class ConfiguracaoResponseDTO
{
    public int Id { get; set; }
    public decimal TaxaJuros { get; set; }
    public int MinParcelas { get; set; }
    public int MaxParcelas { get; set; }
    public decimal PercentualReserva { get; set; }
    public DateTime DataCadastro { get; set; }
}