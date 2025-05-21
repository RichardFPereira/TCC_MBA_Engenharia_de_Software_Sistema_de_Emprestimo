using Microsoft.EntityFrameworkCore;
using Backend.UsuarioService.Models.Entities;

namespace Backend.UsuarioService.Data
{
    public class UsuarioDbContext : DbContext
    {
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Credencial> Credenciais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.CPF).IsUnique();

            modelBuilder.Entity<Credencial>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Credencial>().HasOne(c => c.Usuario).WithOne().HasForeignKey<Credencial>(c => c.UsuarioId);
        }
    }
}