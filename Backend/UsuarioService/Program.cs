using Backend.UsuarioService.Data;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Repositories;
using Backend.UsuarioService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();

// Configurar DbContext com SQL Server
builder.Services.AddDbContext<UsuarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositório
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registrar serviço
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TCC Sistema Empréstimo - Usuário API",
        Version = "v1",
        Description = "API para gerenciamento de usuários do sistema de empréstimo"
    });
});

var app = builder.Build();

// Configurar o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TCC Sistema Empréstimo - Usuário API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();