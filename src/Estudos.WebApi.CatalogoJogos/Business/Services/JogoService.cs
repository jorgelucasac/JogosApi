using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Models;

namespace Estudos.WebApi.CatalogoJogos.Business.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly INotificador _notificador;

        public JogoService(IJogoRepository jogoRepository, INotificador notificador)
        {
            _jogoRepository = jogoRepository;
            _notificador = notificador;
        }

        public IQueryable<Jogo> Query()
        {
            return _jogoRepository.Query();
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
        {
            return await _jogoRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<Jogo>> ObterAsync(int pagina, int quantidade)
        {
            return await _jogoRepository.ObterAsync(pagina, quantidade);
        }

        public async Task<Jogo> ObterPorIdAsync(Guid id)
        {
            return await _jogoRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> ExisteAsync(Guid id)
        {
            return await _jogoRepository.Existe(id);
        }

        public async Task<Jogo> AdicionarAsync(Jogo jogo)
        {
            var jogoAtual = await _jogoRepository
                .BuscarAsync(a => a.Nome == jogo.Nome && a.Produtora == jogo.Produtora);

            if (jogoAtual.Any())
            {
                _notificador.Notificar("Jogo", "Jogo já cadastrado para essa produtora");
                return jogo;
            }

            await _jogoRepository.AdicionarAsync(jogo);
            return jogo;
        }

        public async Task AtualizarAsync(Guid id, Jogo jogo)
        {
            var jogoUpdate = await ObterPorIdAsync(id);
            jogoUpdate.Nome = jogo.Nome;
            jogoUpdate.Produtora = jogo.Produtora;
            jogoUpdate.Preco = jogo.Preco;

            await _jogoRepository.AtualizarAsync(jogoUpdate);
        }

        public async Task<Jogo> RemoverAsync(Guid id)
        {
            var jogo = await ObterPorIdAsync(id);

            if ((DateTime.Now - jogo.DataCadastro).Days > 1)
            {
                _notificador.Notificar("Jogo", "O Jogo só pode ser apagado em até 24h após data de cadastro");
                return jogo;
            }

            jogo.Desativar();
            await _jogoRepository.AtualizarAsync(jogo);
            return jogo;
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}