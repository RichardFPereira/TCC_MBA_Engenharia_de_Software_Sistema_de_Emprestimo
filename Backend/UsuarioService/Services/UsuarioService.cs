using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using BCrypt.Net;

namespace Backend.UsuarioService.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
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
                Email = credencial.Email,
                DataCadastro = usuario.DataCadastro
            };
        }

        public async Task<UsuarioResponseDTO> LoginAsync(LoginDTO dto)
        {
            var credencial = await _repository.GetCredencialByEmailAsync(dto.Email);
            if (credencial == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, credencial.SenhaHash))
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            return new UsuarioResponseDTO
            {
                Id = credencial.Usuario.Id,
                Nome = credencial.Usuario.Nome,
                CPF = credencial.Usuario.CPF,
                DataNascimento = credencial.Usuario.DataNascimento,
                Email = credencial.Email,
                DataCadastro = credencial.Usuario.DataCadastro
            };
        }
    }
}