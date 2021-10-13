using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.WebApi.CatalogoJogos.Filters
{
    public class ResponseNotFound : ProducesResponseTypeAttribute
    {
        public ResponseNotFound() : base(typeof(string), StatusCodes.Status404NotFound)
        {
        }
    }
}