using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Models;
using Estudos.WebApi.CatalogoJogos.Filters;
using Estudos.WebApi.CatalogoJogos.Models.InputModels;
using Estudos.WebApi.CatalogoJogos.Models.Params;
using Estudos.WebApi.CatalogoJogos.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Estudos.WebApi.CatalogoJogos.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class JogosController : BaseApiController
    {
        private readonly IJogoService _jogoService;
        private readonly IMapper _mapper;
        public JogosController(INotificador notificador, IJogoService jogoService, IMapper mapper) : base(notificador)
        {
            _jogoService = jogoService;
            _mapper = mapper;
        }


        /// <summary>
        /// Obtenha todos os jogos
        /// </summary>
        /// <returns>lista de <see cref="JogoViewModel"/></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JogoViewModel>>> ObterTodos()
        {
            var listaJogos = await _jogoService.ObterTodos();
            return ResponseGetList(_mapper.Map<List<JogoViewModel>>(listaJogos));
        }

        /// <summary>
        /// Obtenha os jogos filtrados
        /// </summary>
        /// <returns>lista de <see cref="JogoViewModel"/></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery] JogosParametros parametros)
        {
            var listaJogos = await _jogoService.Obter(parametros);
            return ResponseGetList(_mapper.Map<List<JogoViewModel>>(listaJogos));
        }


        /// <summary>
        /// Obtenha os jogos paginados
        /// </summary>
        /// <returns>lista de <see cref="JogoViewModel"/></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina, [FromQuery, Range(10, 50)] int quantidade)
        {
            var listaJogos = await _jogoService.Obter(pagina, quantidade);
            return ResponseGetList(_mapper.Map<List<JogoViewModel>>(listaJogos));
        }


        /// <summary>
        /// Obtenha os dados de um jogo
        /// </summary>
        /// <returns><see cref="JogoViewModel"/></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JogoViewModel>> Obter(Guid id)
        {
            var jogo = await _jogoService.ObterPorId(id);
            return ResponseGet(_mapper.Map<JogoViewModel>(jogo));
        }

        /// <summary>
        /// realize o cadastro de um jogo
        /// </summary>
        /// <returns><see cref="JogoViewModel"/></returns>
        [HttpPost, ValidacaoModelState]
        [ProducesResponseType(typeof(JogoViewModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<JogoViewModel>> Adicionar(JogoInputModel jogoInput)
        {
            var jogo = _mapper.Map<Jogo>(jogoInput);
            await _jogoService.Adicionar(jogo).ConfigureAwait(false);
            return ResponsePost(nameof(Obter), new { id = jogo.Id }, _mapper.Map<JogoViewModel>(jogo));
        }


        /// <summary>
        /// atualize os dados de um jogo
        /// </summary>
        [HttpPut("{id:guid}"), ValidacaoModelState]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType((typeof(string)), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Atualizar(Guid id, JogoInputModel jogoInput)
        {
            if (await NaoExiste(id)) return ResponseNotFound();

            var jogo = _mapper.Map<Jogo>(jogoInput);
            await _jogoService.Atualizar(id, jogo).ConfigureAwait(false);
            return ResponsePutPatch();
        }

        /// <summary>
        /// atualize o preço de um jogo
        /// </summary>
        [HttpPatch("{id:guid}"), ValidacaoModelState]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType((typeof(string)), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Atualizar(Guid id, JsonPatchDocument<JogoPathInputModel> input)
        {
            if (await NaoExiste(id)) return ResponseNotFound();


            var jogoAtual = await _jogoService.ObterPorId(id).ConfigureAwait(false);
            var jogoViewModel = _mapper.Map<JogoPathInputModel>(jogoAtual);

            input.ApplyTo(jogoViewModel);
            jogoAtual = _mapper.Map<Jogo>(jogoViewModel);

            await _jogoService.Atualizar(id, jogoAtual).ConfigureAwait(false);
            return ResponsePutPatch();
        }

        /// <summary>
        /// remova um jogo
        /// </summary>
        /// <returns><see cref="JogoViewModel"/></returns>
        [HttpPatch("{id:guid}"), ValidacaoModelState]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType((typeof(string)), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JogoViewModel>> Remover(Guid id)
        {
            if (await NaoExiste(id)) return ResponseNotFound();

            var jogo = await _jogoService.Remover(id).ConfigureAwait(false);
            return ResponseDelete(jogo);
        }


        private async Task<bool> NaoExiste(Guid id)
        {
            return !await _jogoService.Existe(id);
        }
    }
}