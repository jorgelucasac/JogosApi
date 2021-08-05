using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estudos.WebApi.CatalogoJogos.Business.Models;

namespace Estudos.WebApi.CatalogoJogos.Business.Interfaces
{
    public interface IRepository<TEntitdade> : IDisposable where TEntitdade : Entidade
    {
        IQueryable<TEntitdade> Query();
        Task<IEnumerable<TEntitdade>> ObterTodosAsync();
        Task<TEntitdade> ObterPorIdAsync(Guid id);
        Task<bool> Existe(Guid id);
        Task AdicionarAsync(TEntitdade entidade);
        Task AtualizarAsync(TEntitdade entidade);
        Task<IEnumerable<TEntitdade>> BuscarAsync(Expression<Func<TEntitdade, bool>> expression);
        Task<int> SaveChangesAsync();
    }
}