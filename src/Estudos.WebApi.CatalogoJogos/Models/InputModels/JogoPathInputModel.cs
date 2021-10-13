using System.ComponentModel.DataAnnotations;

namespace Estudos.WebApi.CatalogoJogos.Models.InputModels
{
    public class JogoPathInputModel
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
        public string Produtora { get; set; }

        [Range(1, 1000, ErrorMessage = "O campo {0} deve ter entre {1} e {2} reais")]
        public double Preco { get; set; }
    }
}