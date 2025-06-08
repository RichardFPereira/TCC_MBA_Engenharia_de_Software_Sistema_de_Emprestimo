using Backend.UsuarioService.Data;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.UsuarioService.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioDbContext _context;

        public UsuarioRepository(UsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByCPFAsync(string cpf)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.CPF == cpf);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Credencial?> GetCredencialByEmailAsync(string email)
        {
            return await _context.Credenciais.Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddUsuarioAsync(Usuario usuario, Credencial credencial)
        {
            _context.Usuarios.AddAsync(usuario);
            _context.Credenciais.AddAsync(credencial);

            await _context.SaveChangesAsync();
        }
    }
}