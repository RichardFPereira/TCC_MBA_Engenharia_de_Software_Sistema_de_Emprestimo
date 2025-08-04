using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Services;

public class LogAcoesService : ILogAcoesService
{
    private readonly ILogAcoesRepository _repository;

    public LogAcoesService(ILogAcoesRepository repository)
    {
        _repository = repository;
    }

    public async Task AddLogAcaoAsync(string acao, string detalhes, int administradorId)
    {
        var log = new LogAcoes
        {
            AdministradorId = administradorId,
            Acao = acao,
            Detalhes = detalhes,
            Data = DateTime.UtcNow
        };

        await _repository.AddLogAcaoAsync(log);
    }

    public async Task<List<LogAcoesResponseDTO>> GetLogsByAdministradorIdAsync(int administradorId)
    {
        var logs = await _repository.GetLogsByAdministradorIdAsync(administradorId);
        var result = new List<LogAcoesResponseDTO>();

        foreach (var log in logs)
        {
            result.Add(new LogAcoesResponseDTO
            {
                Id = log.Id,
                AdministradorId = log.AdministradorId,
                NomeAdministrador = log.Administrador?.Nome ?? string.Empty,
                Acao = log.Acao,
                Detalhes = log.Detalhes,
                Data = log.Data
            });
        }

        return result;
    }
}