using AutoMapper;
using Estudos.WebApi.CatalogoJogos.Configurations;
using Estudos.WebApi.CatalogoJogos.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.WebApi.CatalogoJogos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<JogosContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("WebApiDbConnection"));
            });

            services.AddApiConfiguration();
            services.RegisterServices();

            services.AddSwaggerConfiguration();
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiConfiguration(env);
        }
    }
}
