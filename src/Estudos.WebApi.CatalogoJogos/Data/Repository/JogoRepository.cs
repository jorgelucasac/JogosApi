using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.IQueryable.Extensions.Pagination;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Estudos.WebApi.CatalogoJogos.Data.Repository
{
    public class JogoRepository : Repository<Jogo>, IJogoRepository
    {
        public JogoRepository(JogosContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Jogo>> ObterAsync(int pagina, int quantidade)
        {
            return await Context.Jogos
                .Paginate(quantidade, pagina)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}