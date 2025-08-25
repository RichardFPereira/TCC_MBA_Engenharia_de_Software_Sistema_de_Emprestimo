using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Backend.UsuarioService.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly IConfiguration _configuration;

    public UsuarioService(IUsuarioRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<UsuarioResponseDTO> CadastrarAsync(CreateUsuarioDTO dto)
    {
        if (await _repository.GetByCPFAsync(dto.CPF) != null)
            throw new InvalidOperationException("CPF já cadastrado.");
        if (await _repository.GetCredencialByEmailAsync(dto.Email) != null)
            throw new InvalidOperationException("Email já cadastrado");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            DataNascimento = dto.DataNascimento,
            Salario = dto.Salario ?? 0m,
            Reserva = dto.Reserva ?? 0m,
            Role = "Participante",
            DataCadastro = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow
        };

        var credencial = new Credencial
        {
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            DataCadastro = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow,
            Usuario = usuario
        };

        await _repository.AddUsuarioAsync(usuario, credencial);

        return new UsuarioResponseDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CPF = usuario.CPF,
            DataNascimento = usuario.DataNascimento,
            Salario = usuario.Salario,
            Reserva = usuario.Reserva,
            Role = usuario.Role,
            Email = credencial.Email,
            DataCadastro = usuario.DataCadastro
        };
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
    {
        var credencial = await _repository.GetCredencialByEmailAsync(dto.Email);
        if (credencial == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, credencial.SenhaHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos.");

        var token = GenerateJwtToken(credencial);

        return new LoginResponseDTO
        {
            Token = token,
            Usuario = new UsuarioResponseDTO
            {
                Id = credencial.Usuario.Id,
                Nome = credencial.Usuario.Nome,
                CPF = credencial.Usuario.CPF,
                DataNascimento = credencial.Usuario.DataNascimento,
                Salario = credencial.Usuario.Salario,
                Reserva = credencial.Usuario.Reserva,
                Role = credencial.Usuario.Role,
                Email = credencial.Email,
                DataCadastro = credencial.Usuario.DataCadastro
            }
        };
    }

    private string GenerateJwtToken(Credencial credencial)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, credencial.Usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, credencial.Email),
            new Claim(ClaimTypes.Role, credencial.Usuario.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}