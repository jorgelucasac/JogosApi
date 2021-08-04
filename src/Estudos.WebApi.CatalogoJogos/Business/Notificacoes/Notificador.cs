using System;
using System.Collections.Generic;
using System.Linq;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;

namespace Estudos.WebApi.CatalogoJogos.Business.Notificacoes
{
    public class Notificador : INotificador
    {
        private readonly IList<Notificacao> _notificacoes;

        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        public bool TemNotificacoes()
        {
            return _notificacoes.Any();
        }

        public IList<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        public void Notificar(Notificacao notificacao)
        {
            throw new NotImplementedException();
        }

    }
}