using System;
using System.Collections.Generic;
using System.Linq;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.WebApi.CatalogoJogos.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly INotificador _notificador;

        protected BaseApiController(INotificador notificador)
        {
            _notificador = notificador;
        }
        

        protected bool OperacaoValida()
        {
            return (!_notificador.TemNotificacoes());
        }


        protected ActionResult ResponsePutPatch()
        {
            if (OperacaoValida())
            {
                return NoContent();
            }

            return BadRequest(new ValidationProblemDetails(ObterNotificaCacoesPorChave()));
        }

        protected ActionResult<T> ResponseDelete<T>(T item)
        {
            if (!OperacaoValida())
                return BadRequest(ObterErrosResposta());
            if (item == null)
                return NoContent();

            return Ok(item);

        }

        protected ActionResult<T> ResponsePost<T>(string action, object route, T result)
        {
            if (OperacaoValida())
            {
                if (result == null)
                    return NoContent();

                return CreatedAtAction(action, route, result);
            }

            return BadRequest(ObterErrosResposta());
        }

        protected ActionResult<T> ResponsePost<T>(string action, string controller, object route, T result)
        {
            if (OperacaoValida())
            {
                if (result == null)
                    return NoContent();

                return CreatedAtAction(action, controller, route, result);
            }

            return BadRequest(ObterErrosResposta());
        }
        protected ActionResult<IEnumerable<T>> ResponseGet<T>(IEnumerable<T> result)
        {

            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }

        protected ActionResult<T> ResponseGet<T>(T result)
        {
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        
        protected ActionResult ModelStateErroResponse()
        {
            NotificarErrosModelState();
            return BadRequest();
        }

        protected void NotificarErro(string chave, string mensagem)
        {
            _notificador.Notificar(new Notificacao(chave, mensagem));
        }

        private void NotificarErrosModelState()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(string.Empty, erroMsg);
            }
        }

        private  Dictionary<string, string[]> ObterNotificaCacoesPorChave()
        {
            var strings = _notificador.ObterNotificacoes().Select(s => s.Chave).Distinct();
            var dictionary = new Dictionary<string, string[]>();
            foreach (var str in strings)
            {
                var key = str;
                dictionary[key] = this._notificador.ObterNotificacoes()
                    .Where(w => w.Chave.Equals(key, StringComparison.Ordinal))
                    .Select(s => s.Valor).ToArray();
            }
            return dictionary;
        }

        private ValidationProblemDetails ObterErrosResposta()
        {
            return new ValidationProblemDetails(ObterNotificaCacoesPorChave())
            {
                Title = "Erros de validação encontrados"
            };
        }
    }
}