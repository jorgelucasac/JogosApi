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
        Task<IEnumerable<TEntitdade>> ObterTodos();
        Task<TEntitdade> ObterPorId(Guid id);
        Task<bool> Existe(Guid id);
        Task Adicionar(TEntitdade entidade);
        Task Atualizar(Guid id, TEntitdade entidade);
        Task<TEntitdade> Remover(Guid id);
        Task<IEnumerable<TEntitdade>> Buscar(Expression<Func<TEntitdade, bool>> expression);
        Task<int> SaveChanges();
    }
}