using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AspNetCore.IQueryable.Extensions;
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
using Microsoft.EntityFrameworkCore;

namespace Estudos.WebApi.CatalogoJogos.Controllers.V1
{

    [ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
        /// <response code="200">Retorna todos o jogos</response>
        /// <response code="204">Caso não exista registros</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JogoViewModel>>> ObterTodos()
        {
            var listaJogos = await _jogoService.ObterTodosAsync();
            return ResponseGetList(_mapper.Map<List<JogoViewModel>>(listaJogos));
        }

        /// <summary>
        /// Obtenha os jogos filtrados
        /// </summary>
        /// <returns>lista de <see cref="JogoViewModel"/></returns>
        /// <response code="200">Retorna os jogos encontrados</response>
        /// <response code="204">Caso não exista registros para os parametros informados</response>
        [HttpGet("busca-parametrizada")]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery] JogosParametros parametros)
        {
            var listaJogos = _jogoService.Query().Apply(parametros);
            return ResponseGetList(await _mapper.ProjectTo<JogoViewModel>(listaJogos).ToListAsync());
        }


        /// <summary>
        /// Obtenha os jogos paginados
        /// </summary>
        /// 
        /// <returns>lista de <see cref="JogoViewModel"/></returns>
        /// <param name="pagina">Pagina desejada</param>
        /// <param name="quantidade">Quantidade de itens por página</param>
        /// <response code="200">Retorna os jogos encontrados</response>
        /// <response code="204">Caso não exista registros</response>
        [HttpGet("obter-paginados")]
        [ProducesResponseType(typeof(List<JogoViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina, [FromQuery, Range(10, 50)] int quantidade)
        {
            var listaJogos = await _jogoService.ObterAsync(pagina, quantidade);
            return ResponseGetList(_mapper.Map<List<JogoViewModel>>(listaJogos));
        }


        /// <summary>
        /// Obtenha os dados de um jogo
        /// </summary>
        /// <param name="id">Id do jogo desejado</param>
        /// <returns><see cref="JogoViewModel"/></returns>
        /// <response code="200">Retorna o jogo encontrado</response>
        /// <response code="404">Caso não exista registro com com o id informado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JogoViewModel), StatusCodes.Status200OK)]
        [ResponseNotFound]
        public async Task<ActionResult<JogoViewModel>> ObterPorId(Guid id)
        {
            var jogo = await _jogoService.ObterPorIdAsync(id);
            return ResponseGet(_mapper.Map<JogoViewModel>(jogo));
        }

        /// <summary>
        /// realize o cadastro de um jogo
        /// </summary>
        /// <returns><see cref="JogoViewModel"/></returns>
        /// <response code="201">Retorna o jogo cadastrado</response>
        /// <response code="400">Caso alguma validação não seja atendida</response>
        [HttpPost, ValidacaoModelState]
        [ProducesResponseType(typeof(JogoViewModel), StatusCodes.Status201Created)]
        [ResponseBadRequest]
        public async Task<ActionResult<JogoViewModel>> Adicionar(JogoInputModel jogoInput)
        {
            var jogo = _mapper.Map<Jogo>(jogoInput);
            await _jogoService.AdicionarAsync(jogo).ConfigureAwait(false);
            return ResponsePost(nameof(ObterPorId), new { id = jogo.Id }, _mapper.Map<JogoViewModel>(jogo));
        }


        /// <summary>
        /// atualize os dados de um jogo
        /// </summary>
        /// <param name="id">Id do jogo que será atualizado</param>
        /// <param name="jogoInput">novos dados do jogo</param>
        /// <response code="204">Quando o jogo for atualizado com sucesso</response>
        /// <response code="404">Caso não exista registro com com o id informado</response>
        /// <response code="400">Caso alguma validação não seja atendida</response>
        [HttpPut("{id:guid}"), ValidacaoModelState]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseNotFound, ResponseBadRequest]
        public async Task<ActionResult> Atualizar(Guid id, JogoInputModel jogoInput)
        {
            if (await NaoExiste(id)) return ResponseNotFound();

            var jogo = _mapper.Map<Jogo>(jogoInput);
            await _jogoService.AtualizarAsync(id, jogo).ConfigureAwait(false);
            return ResponsePutPatch();
        }

        /// <summary>
        /// atualize o parcialmente um jogo
        /// </summary>
        /// <param name="id">Id do jogo que será atualizado</param>
        /// <param name="input">campo/valor a ser atualizado</param>
        /// <response code="204">Quando o jogo for atualizado com sucesso</response>
        /// <response code="404">Caso não exista registro com com o id informado</response>
        /// <response code="400">Caso alguma validação não seja atendida</response>
        [HttpPatch("{id:guid}"), ValidacaoModelState]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseNotFound, ResponseBadRequest]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] JsonPatchDocument<JogoPathInputModel> input)
        {
            if (await NaoExiste(id)) return ResponseNotFound();


            var jogoAtual = await _jogoService.ObterPorIdAsync(id).ConfigureAwait(false);
            var jogoViewModel = _mapper.Map<JogoPathInputModel>(jogoAtual);

            input.ApplyTo(jogoViewModel);
            jogoAtual = _mapper.Map<Jogo>(jogoViewModel);

            await _jogoService.AtualizarAsync(id, jogoAtual).ConfigureAwait(false);
            return ResponsePutPatch();
        }

        /// <summary>
        /// remova um jogo
        /// </summary>
        /// <param name="id">Id do jogo que será removido</param>
        /// <returns><see cref="JogoViewModel"/></returns>
        /// <response code="204">Quando o jogo for atualizado com sucesso</response>
        /// <response code="404">Caso não exista registro com com o id informado</response>
        /// <response code="400">Caso alguma validação não seja atendida</response>
        [HttpDelete("{id:guid}"), ValidacaoModelState]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseNotFound, ResponseBadRequest]
        public async Task<ActionResult<JogoViewModel>> Remover(Guid id)
        {
            if (await NaoExiste(id)) return ResponseNotFound();

            var jogo = await _jogoService.RemoverAsync(id).ConfigureAwait(false);
            return ResponseDelete(_mapper.Map<JogoViewModel>(jogo));
        }


        private async Task<bool> NaoExiste(Guid id)
        {
            return !await _jogoService.ExisteAsync(id);
        }
    }
}