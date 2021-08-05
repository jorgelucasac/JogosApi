using System;

namespace Estudos.WebApi.CatalogoJogos.Business.Models
{
    public abstract class Entidade
    {
        public Guid Id { get; }
        public DateTime DataCadastro { get; }
        public bool Ativo { get; private set; }

        protected Entidade()
        {
            Id = Guid.NewGuid();
            DataCadastro = DateTime.Now;
            Ativo = true;
        }

        public void Desativar()
        {
            Ativo = false;
        }
    }
}