using AutoMapper;
using Estudos.WebApi.CatalogoJogos.Business.Models;
using Estudos.WebApi.CatalogoJogos.Models.InputModels;
using Estudos.WebApi.CatalogoJogos.Models.ViewModels;

namespace Estudos.WebApi.CatalogoJogos.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Jogo, JogoViewModel>();
            CreateMap<Jogo, JogoPathInputModel>();

            CreateMap<JogoInputModel, Jogo>();
            CreateMap<JogoPathInputModel, Jogo>();
        }


    }
}
