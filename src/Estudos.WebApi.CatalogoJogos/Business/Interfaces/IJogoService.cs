using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.WebApi.CatalogoJogos.Business.Models;
using Estudos.WebApi.CatalogoJogos.Models.Params;
using Estudos.WebApi.CatalogoJogos.Models.ViewModels;

namespace Estudos.WebApi.CatalogoJogos.Business.Interfaces
{
    public interface IJogoService
    {
        IQueryable<Jogo> Query();
        Task<IEnumerable<Jogo>> ObterTodos();
        Task<IEnumerable<Jogo>> Obter(JogosParametros jogosParametros);
        Task<IEnumerable<Jogo>> Obter(int pagina, int quantidade);
        Task<Jogo> ObterPorId(Guid id);
        Task<bool> Existe(Guid id);
        Task Adicionar(Jogo jogo);
        Task Atualizar(Guid id, Jogo jogo);
        Task<JogoViewModel> Remover(Guid id);
    }
}