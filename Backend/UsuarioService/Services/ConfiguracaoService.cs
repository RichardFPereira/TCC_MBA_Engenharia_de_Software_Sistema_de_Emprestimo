using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using System;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Services;

public class ConfiguracaoService : IConfiguracaoService
{
    private readonly IConfiguracaoRepository _repository;

    public ConfiguracaoService(IConfiguracaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConfiguracaoResponseDTO> CriarConfiguracaoAsync(CreateConfiguracaoDTO dto)
    {
        if (dto.MinParcelas > dto.MaxParcelas)
            throw new InvalidOperationException("O número mínimo de parcelas não pode ser maior que o máximo.");

        var configuracao = new Configuracao
        {
            TaxaJuros = dto.TaxaJuros,
            MinParcelas = dto.MinParcelas,
            MaxParcelas = dto.MaxParcelas,
            PercentualReserva = dto.PercentualReserva,
            DataCadastro = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow
        };

        await _repository.AddConfiguracaoAsync(configuracao);

        return new ConfiguracaoResponseDTO
        {
            Id = configuracao.Id,
            TaxaJuros = configuracao.TaxaJuros,
            MinParcelas = configuracao.MinParcelas,
            MaxParcelas = configuracao.MaxParcelas,
            PercentualReserva = configuracao.PercentualReserva,
            DataCadastro = configuracao.DataCadastro
        };
    }

    public async Task<ConfiguracaoResponseDTO> GetConfiguracaoMaisRecenteAsync()
    {
        var configuracao = await _repository.GetConfiguracaoMaisRecenteAsync();
        if (configuracao == null)
            throw new InvalidOperationException("Nenhuma configuração encontrada.");

        return new ConfiguracaoResponseDTO
        {
            Id = configuracao.Id,
            TaxaJuros = configuracao.TaxaJuros,
            MinParcelas = configuracao.MinParcelas,
            MaxParcelas = configuracao.MaxParcelas,
            PercentualReserva = configuracao.PercentualReserva,
            DataCadastro = configuracao.DataCadastro
        };
    }
}