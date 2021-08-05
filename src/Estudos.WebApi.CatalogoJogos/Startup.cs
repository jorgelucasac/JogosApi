using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Estudos.WebApi.CatalogoJogos.Business.Interfaces;
using Estudos.WebApi.CatalogoJogos.Business.Notificacoes;
using Estudos.WebApi.CatalogoJogos.Business.Services;
using Estudos.WebApi.CatalogoJogos.Data;
using Estudos.WebApi.CatalogoJogos.Data.Repository;
using Estudos.WebApi.CatalogoJogos.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

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

            services.AddControllers()
                .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(typeof(Startup));


            services.AddDbContext<JogosContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("WebApiDbConnection"));
            });


            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<JogosContext>();
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IJogoRepository, JogoRepository>();
            services.AddScoped<IJogoService, JogoService>();



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Estudos.WebApi.CatalogoJogos", Version = "v1" });

                // Definindo o caminho de comentários para o Swagger JSON e UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estudos.WebApi.CatalogoJogos v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
