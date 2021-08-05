using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Notificacoes;
using Estudos.WebApi.CatalogoJogos.Business.Services;
using Estudos.WebApi.CatalogoJogos.Data;
using Estudos.WebApi.CatalogoJogos.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.WebApi.CatalogoJogos.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<JogosContext>();
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IJogoRepository, JogoRepository>();
            services.AddScoped<IJogoService, JogoService>();
        }
    }
}