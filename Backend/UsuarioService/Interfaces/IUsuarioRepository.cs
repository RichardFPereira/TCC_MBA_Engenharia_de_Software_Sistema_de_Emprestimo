using Backend.UsuarioService.Models.Entities;

namespace Backend.UsuarioService.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByCPFAsync(string cpf);
        Task<Usuario?> GetByIdAsync(int id);
        Task<Credencial?> GetCredencialByEmailAsync(string email);
        Task AddUsuarioAsync(Usuario usuario, Credencial credencial);
    }
}