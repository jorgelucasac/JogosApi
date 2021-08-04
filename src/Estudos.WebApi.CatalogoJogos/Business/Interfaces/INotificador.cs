using System.Collections.Generic;
using Estudos.WebApi.CatalogoJogos.Business.Notificacoes;

namespace Estudos.WebApi.CatalogoJogos.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacoes();

        IList<Notificacao> ObterNotificacoes();
        void Notificar(Notificacao notificacao);
    }
}