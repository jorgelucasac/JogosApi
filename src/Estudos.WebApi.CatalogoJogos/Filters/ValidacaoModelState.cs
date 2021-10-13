using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Estudos.WebApi.CatalogoJogos.Filters
{
    public class ValidacaoModelState : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(ObterErros(context.ModelState));
        }

        private ValidationProblemDetails ObterErros(ModelStateDictionary modelState)
        {
            var keys = modelState.Keys.Distinct();
            var dictionary = new Dictionary<string, string[]>();
            foreach (var key in keys)
                dictionary[key] = modelState[key].Errors
                    .Select(erro => erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message)
                    .ToArray();
            return ObterErrosResposta(dictionary);
        }


        private ValidationProblemDetails ObterErrosResposta(Dictionary<string, string[]> erros)
        {
            return new ValidationProblemDetails(erros)
            {
                Title = "Erros de validação encontrados"
            };
        }
    }
}