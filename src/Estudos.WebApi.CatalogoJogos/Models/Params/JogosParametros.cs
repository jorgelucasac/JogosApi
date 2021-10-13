using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.WebApi.CatalogoJogos.Models.Params
{
    public class JogosParametros : IQuerySort, IQueryPaging
    {
        [QueryOperator(Operator = WhereOperator.Contains)]
        [FromQuery(Name = "nome")]
        public string Nome { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains)]
        [FromQuery(Name = "produtora")]
        public string Produtora { get; set; }

        [QueryOperator(Operator = WhereOperator.GreaterThanOrEqualTo, HasName = "Preco")]
        [FromQuery(Name = "preco_minimo")]
        public double? PrecoMinimo { get; set; }

        [QueryOperator(Operator = WhereOperator.LessThanOrEqualTo, HasName = "Preco")]
        [FromQuery(Name = "preco_maximo")]
        public double? PrecoMaximo { get; set; }

        [QueryOperator(Operator = WhereOperator.Equals, HasName = "Preco")]
        [FromQuery(Name = "preco")]
        public double? Preco { get; set; }

        public int? Offset { get; set; }
        public int? Limit { get; set; } = 10;
        public string Sort { get; set; }
    }
}