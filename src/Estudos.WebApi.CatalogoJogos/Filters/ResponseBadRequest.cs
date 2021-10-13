using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.WebApi.CatalogoJogos.Filters
{
    public class ResponseBadRequest : ProducesResponseTypeAttribute
    {
        public ResponseBadRequest() : base(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)
        {
        }
    }
}