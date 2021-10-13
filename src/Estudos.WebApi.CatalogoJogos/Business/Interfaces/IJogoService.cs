using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.WebApi.CatalogoJogos.Business.Models;

namespace Estudos.WebApi.CatalogoJogos.Business.Interfaces
{
    public interface IJogoService : IDisposable
    {
        IQueryable<Jogo> Query();
        Task<IEnumerable<Jogo>> ObterTodosAsync();
        Task<IEnumerable<Jogo>> ObterAsync(int pagina, int quantidade);
        Task<Jogo> ObterPorIdAsync(Guid id);
        Task<bool> ExisteAsync(Guid id);
        Task<Jogo> AdicionarAsync(Jogo jogo);
        Task AtualizarAsync(Guid id, Jogo jogo);
        Task<Jogo> RemoverAsync(Guid id);
    }
}