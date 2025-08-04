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
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }
        public DbSet<Configuracao> Configuracoes { get; set; }
        public DbSet<LogAcoes> LogAcoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.CPF).IsUnique();

            modelBuilder.Entity<Credencial>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Credencial)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Credencial>(c => c.UsuarioId);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);

            modelBuilder.Entity<Parcela>()
                .HasOne(p => p.Emprestimo)
                .WithMany()
                .HasForeignKey(p => p.EmprestimoId);

            modelBuilder.Entity<LogAcoes>()
                .HasOne(l => l.Administrador)
                .WithMany()
                .HasForeignKey(l => l.AdministradorId);
        }
    }
}