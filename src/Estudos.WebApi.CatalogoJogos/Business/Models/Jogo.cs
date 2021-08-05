namespace Estudos.WebApi.CatalogoJogos.Business.Models
{
    public class Jogo : Entidade
    {
        public string Nome { get; set; }
        public string Produtora { get; set; }
        public double Preco { get; set; }
    }
}