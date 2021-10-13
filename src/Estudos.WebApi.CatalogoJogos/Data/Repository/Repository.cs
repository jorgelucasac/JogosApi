using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Estudos.WebApi.CatalogoJogos.Data.Repository
{
    public abstract class Repository<TEntidade> : IRepository<TEntidade> where TEntidade : Entidade, new()
    {
        protected readonly JogosContext Context;
        protected readonly DbSet<TEntidade> DbSet;

        protected Repository(JogosContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntidade>();
        }

        public IQueryable<TEntidade> Query()
        {
            return DbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntidade>> ObterTodosAsync()
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().ToListAsync();
        }

        public async Task<TEntidade> ObterPorIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<bool> Existe(Guid id)
        {
            return await DbSet.AnyAsync(a => a.Id == id);
        }

        public async Task AdicionarAsync(TEntidade entidade)
        {
            await DbSet.AddAsync(entidade);
            await SaveChangesAsync();
        }

        public async Task AtualizarAsync(TEntidade entidade)
        {
            DbSet.Update(entidade);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntidade>> BuscarAsync(Expression<Func<TEntidade, bool>> expression)
        {
            return await DbSet.Where(expression).AsNoTrackingWithIdentityResolution().ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}