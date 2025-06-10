using System.ComponentModel.DataAnnotations;

namespace Backend.UsuarioService.DTOs;

public class CreateUsuarioDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve ter 11 dígitos numéricos.")]
    public string CPF { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? DataNascimento { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 20 caracteres.")]
    public string Senha { get; set; } = string.Empty;
}

public class LoginDTO
{
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;
}

public class UsuarioResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
}