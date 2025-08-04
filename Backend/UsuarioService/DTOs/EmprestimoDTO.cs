using System.ComponentModel.DataAnnotations;

namespace Backend.UsuarioService.DTOs;

public class CreateEmprestimoDTO
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O número de parcelas é obrigatório.")]
    [Range(4, 72, ErrorMessage = "O número de parcelas deve estar entre 4 e 72.")]
    public int NumeroParcelas { get; set; }
}

public class EmprestimoResponseDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal ValorTotal { get; set; }
    public int NumeroParcelas { get; set; }
    public decimal TaxaJuros { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
}

public class ParcelaResponseDTO
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public int NumeroParcela { get; set; }
    public decimal ValorParcela { get; set; }
    public DateTime DataVencimento { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DataPagamento { get; set; }
}