﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using Whatflix.Infrastructure.Injection;
using Whatflix.Infrastructure.Mapping;
using Whatflix.Infrastructure.ServiceSettings;
using Whatflix.Presentation.Api.Helpers;

namespace Whatflix.Api
{
    public class Startup
    {
        private InjectionModule _injectionModule;
        private MappingModule _mappingModule;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _injectionModule = new InjectionModule();
            _mappingModule = new MappingModule();

            Console.WriteLine("--- CONFIGURATION ---");
            Configuration.AsEnumerable().ToList().ForEach(i => Console.WriteLine("{0}", i));
            Console.WriteLine("--- CONFIGURATION ---");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<SettingsWrapper>(Configuration);
            services.AddScoped<ControllerHelper>();

            _injectionModule.ConfigureServices(services);
            _injectionModule.ConfigureRepositories(services, shouldUseMongoRepository: bool.Parse(Configuration["Repositories:UseMongoRepository"]));
            _mappingModule.ConfigureServices(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Version = "v1.0", Title = "Whatflix", });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Whatflix.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "whatflix/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/whatflix/v1.0/swagger.json", "Whatflix Api v1.0");
            });
        }
    }
}
