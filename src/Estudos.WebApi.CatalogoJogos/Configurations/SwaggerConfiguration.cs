using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Estudos.WebApi.CatalogoJogos.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Definindo o caminho de comentários para o Swagger JSON e UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<SwaggerDefaultValues>();
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //gera um end point para cada versao
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", "Api Jogos -" + description.GroupName.ToUpperInvariant());
                }
            });
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            //add um doc para cada versão da api
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        //documentação com informação básica para cada versão
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Jogos API",
                Description = "Api de estudos REST",
                Contact = new OpenApiContact()
                {
                    Name = "Jorge Lucas",
                    Email = "jorgelucasl91@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/jorge-lucas/")
                },
                TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " <b style='color: red;'>Esta versão está obsoleta!</b>";
            }

            return info;
        }
    }


    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            //deixa em cinza os métodos obsoletos
            operation.Deprecated = apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription
                    .ParameterDescriptions
                    .First(p => p.Name == parameter.Name);

                var routeInfo = description.RouteInfo;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }



                if (routeInfo == null)
                {
                    continue;
                }

                if (parameter.In != ParameterLocation.Path && parameter.Schema.Default == null)
                {
                    parameter.Schema.Default = new OpenApiString(routeInfo.DefaultValue.ToString());
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }
}