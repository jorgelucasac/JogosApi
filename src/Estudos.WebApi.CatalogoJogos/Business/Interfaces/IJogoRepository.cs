using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.WebApi.CatalogoJogos.Business.Models;

namespace Estudos.WebApi.CatalogoJogos.Business.Interfaces
{
    public interface IJogoRepository : IRepository<Jogo>
    {
        Task<IEnumerable<Jogo>> ObterAsync(int pagina, int quantidade);
    }
}