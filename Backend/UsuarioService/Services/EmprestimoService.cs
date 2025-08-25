using Backend.UsuarioService.DTOs;
using Backend.UsuarioService.Interfaces;
using Backend.UsuarioService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.UsuarioService.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _emprestimoRepository;
    private readonly IConfiguracaoRepository _configuracaoRepository;
    private readonly ILogAcoesRepository _logAcoesRepository;

    public EmprestimoService(IEmprestimoRepository emprestimoRepository, IConfiguracaoRepository configuracaoRepository, ILogAcoesRepository logAcoesRepository)
    {
        _emprestimoRepository = emprestimoRepository;
        _configuracaoRepository = configuracaoRepository;
        _logAcoesRepository = logAcoesRepository;
    }

    public async Task<EmprestimoResponseDTO> CriarEmprestimoAsync(int usuarioId, CreateEmprestimoDTO dto)
    {
        var usuario = await _emprestimoRepository.GetUsuarioByIdAsync(usuarioId);
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado.");

        if (usuario.Role != "Participante")
            throw new InvalidOperationException("Apenas participantes podem solicitar empréstimos.");

        if (await _emprestimoRepository.HasEmprestimoAtivoAsync(usuarioId))
            throw new InvalidOperationException("Usuário já possui um empréstimo ativo.");

        var configuracao = await _configuracaoRepository.GetConfiguracaoMaisRecenteAsync();
        if (configuracao == null)
            throw new InvalidOperationException("Configuração não encontrada.");

        if (dto.NumeroParcelas < configuracao.MinParcelas || dto.NumeroParcelas > configuracao.MaxParcelas)
            throw new InvalidOperationException($"O número de parcelas deve estar entre {configuracao.MinParcelas} e {configuracao.MaxParcelas}.");

        var valorMaximo = usuario.Reserva * configuracao.PercentualReserva;
        if (dto.Valor > valorMaximo)
            throw new InvalidOperationException($"O valor do empréstimo excede o limite de {valorMaximo:C}.");

        var valorTotal = dto.Valor + (dto.Valor * configuracao.TaxaJuros * dto.NumeroParcelas);
        var valorParcela = valorTotal / dto.NumeroParcelas;
        if (valorParcela > usuario.Salario * 0.3m)
            throw new InvalidOperationException("A parcela excede 30% do salário.");

        var emprestimo = new Emprestimo
        {
            UsuarioId = usuarioId,
            Valor = dto.Valor,
            ValorTotal = valorTotal,
            NumeroParcelas = dto.NumeroParcelas,
            TaxaJuros = configuracao.TaxaJuros,
            DataEmprestimo = DateTime.UtcNow,
            Status = "Pendente",
            DataCadastro = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow
        };

        var parcelas = new List<Parcela>();
        for (int i = 1; i <= dto.NumeroParcelas; i++)
        {
            parcelas.Add(new Parcela
            {
                NumeroParcela = i,
                ValorParcela = valorParcela,
                DataVencimento = DateTime.UtcNow.AddMonths(i),
                Status = "Pendente",
                DataPagamento = null
            });
        }

        await _emprestimoRepository.AddEmprestimoAsync(emprestimo, parcelas);

        return new EmprestimoResponseDTO
        {
            Id = emprestimo.Id,
            UsuarioId = emprestimo.UsuarioId,
            NomeUsuario = usuario.Nome,
            Valor = emprestimo.Valor,
            ValorTotal = emprestimo.ValorTotal,
            NumeroParcelas = emprestimo.NumeroParcelas,
            TaxaJuros = emprestimo.TaxaJuros,
            DataEmprestimo = emprestimo.DataEmprestimo,
            Status = emprestimo.Status,
            DataCadastro = emprestimo.DataCadastro
        };
    }

    public async Task<List<EmprestimoResponseDTO>> ListarEmprestimosPorUsuarioAsync(int usuarioId)
    {
        var emprestimos = await _emprestimoRepository.GetEmprestimosByUsuarioIdAsync(usuarioId);
        var result = new List<EmprestimoResponseDTO>();

        foreach (var emprestimo in emprestimos)
        {
            result.Add(new EmprestimoResponseDTO
            {
                Id = emprestimo.Id,
                UsuarioId = emprestimo.UsuarioId,
                NomeUsuario = emprestimo.Usuario?.Nome ?? string.Empty,
                Valor = emprestimo.Valor,
                ValorTotal = emprestimo.ValorTotal,
                NumeroParcelas = emprestimo.NumeroParcelas,
                TaxaJuros = emprestimo.TaxaJuros,
                DataEmprestimo = emprestimo.DataEmprestimo,
                Status = emprestimo.Status,
                DataCadastro = emprestimo.DataCadastro
            });
        }

        return result;
    }

    public async Task<EmprestimoResponseDTO> AutorizarEmprestimoAsync(int id, bool autorizar)
    {
        var emprestimo = await _emprestimoRepository.GetEmprestimoByIdAsync(id);
        if (emprestimo == null)
            throw new InvalidOperationException("Empréstimo não encontrado.");

        if (emprestimo.Status != "Pendente")
            throw new InvalidOperationException("Empréstimo não está pendente.");

        emprestimo.Status = autorizar ? "Em Andamento" : "Cancelado";
        emprestimo.DataAlteracao = DateTime.UtcNow;

        await _emprestimoRepository.UpdateEmprestimoAsync(emprestimo);

        await _logAcoesRepository.AddLogAcaoAsync(new LogAcoes
        {
            AdministradorId = emprestimo.UsuarioId,
            Acao = autorizar ? "Autorizar Empréstimo" : "Rejeitar Empréstimo",
            Detalhes = $"Empréstimo ID {id} {emprestimo.Status} para usuário ID {emprestimo.UsuarioId}",
            Data = DateTime.UtcNow
        });

        return new EmprestimoResponseDTO
        {
            Id = emprestimo.Id,
            UsuarioId = emprestimo.UsuarioId,
            NomeUsuario = emprestimo.Usuario?.Nome ?? string.Empty,
            Valor = emprestimo.Valor,
            ValorTotal = emprestimo.ValorTotal,
            NumeroParcelas = emprestimo.NumeroParcelas,
            TaxaJuros = emprestimo.TaxaJuros,
            DataEmprestimo = emprestimo.DataEmprestimo,
            Status = emprestimo.Status,
            DataCadastro = emprestimo.DataCadastro
        };
    }

    public async Task<ParcelaResponseDTO> AtualizarStatusParcelaAsync(int emprestimoId, int parcelaId, UpdateParcelaStatusDTO dto)
    {
        var parcela = await _emprestimoRepository.GetParcelaByIdAsync(parcelaId);
        if (parcela == null || parcela.EmprestimoId != emprestimoId)
            throw new InvalidOperationException("Parcela não encontrada ou não pertence ao empréstimo.");

        if (parcela.Status == "Pago")
            throw new InvalidOperationException("Parcela já foi paga.");

        parcela.Status = dto.Status;
        if (dto.Status == "Pago")
            parcela.DataPagamento = DateTime.UtcNow;

        await _emprestimoRepository.UpdateParcelaAsync(parcela);

        if (await _emprestimoRepository.AllParcelasPagasAsync(emprestimoId))
        {
            var emprestimo = await _emprestimoRepository.GetEmprestimoByIdAsync(emprestimoId);
            if (emprestimo != null && emprestimo.Status != "Pago")
            {
                emprestimo.Status = "Pago";
                emprestimo.DataAlteracao = DateTime.UtcNow;
                await _emprestimoRepository.UpdateEmprestimoAsync(emprestimo);

                await _logAcoesRepository.AddLogAcaoAsync(new LogAcoes
                {
                    AdministradorId = emprestimo.UsuarioId,
                    Acao = "Fechar Empréstimo",
                    Detalhes = $"Empréstimo ID {emprestimoId} marcado como Pago",
                    Data = DateTime.UtcNow
                });
            }
        }

        return new ParcelaResponseDTO
        {
            Id = parcela.Id,
            EmprestimoId = parcela.EmprestimoId,
            NumeroParcela = parcela.NumeroParcela,
            ValorParcela = parcela.ValorParcela,
            DataVencimento = parcela.DataVencimento,
            Status = parcela.Status,
            DataPagamento = parcela.DataPagamento
        };
    }

    public async Task<List<object>> GetEmprestimosPendentesAsync()
    {
        var emprestimos = await _emprestimoRepository.GetEmprestimosPendentesAsync();
        var result = new List<object>();

        foreach (var emprestimo in emprestimos)
        {
            result.Add(new
            {
                id = emprestimo.Id,
                usuarioId = emprestimo.UsuarioId,
                nomeUsuario = emprestimo.Usuario?.Nome ?? string.Empty,
                email = emprestimo.Usuario?.Credencial?.Email ?? string.Empty,
                valorReserva = emprestimo.Usuario?.Reserva ?? 0m,
                salario = emprestimo.Usuario?.Salario ?? 0m,
                valor = emprestimo.Valor,
                dataEmprestimo = emprestimo.DataEmprestimo,
                status = emprestimo.Status,
                numeroParcelas = emprestimo.NumeroParcelas
            });
        }

        return result;
    }

    public async Task ProcessarAutorizacoesBatchAsync(List<AutorizacaoBatchDTO> dtoList)
    {
        foreach (var dto in dtoList)
        {
            var emprestimo = await _emprestimoRepository.GetEmprestimoByIdAsync(dto.Id);
            if (emprestimo == null)
                continue;

            if (emprestimo.Status != "Pendente")
                continue;

            emprestimo.Status = dto.Autorizar ? "Em Andamento" : "Cancelado";
            emprestimo.DataAlteracao = DateTime.UtcNow;

            await _emprestimoRepository.UpdateEmprestimoAsync(emprestimo);

            await _logAcoesRepository.AddLogAcaoAsync(new LogAcoes
            {
                AdministradorId = emprestimo.UsuarioId,
                Acao = dto.Autorizar ? "Autorizar Empréstimo" : "Rejeitar Empréstimo",
                Detalhes = $"Empréstimo ID {dto.Id} {emprestimo.Status} para usuário ID {emprestimo.UsuarioId}",
                Data = DateTime.UtcNow
            });
        }
    }
}