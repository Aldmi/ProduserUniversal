using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbstractProduser.Enums;
using AbstractProduser.Options;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProdusersMediator;
using WebApi.AutofacModules;
using WebApi.Hubs;

namespace WebApi
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
            services.AddOptions();
            services.Configure<ProduserOptionAgregator>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.AddCors(options =>
            {
                // задаём политику CORS, чтобы наше клиентское приложение могло отправить запрос на сервер API
                options.AddPolicy("default", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            try
            {
                builder.RegisterModule(new ProdusersUnionAutofacModule());
                builder.RegisterModule(new ExchangeAutofacModule());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка Регистрации зависимостей в DI контейнере {ex}");
                throw;
            }
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILifetimeScope scope)
        {    
            InitializeAsync(scope).Wait();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ProviderHub>("/providerHub");
            });
            app.UseMvc();
        }


        private async Task InitializeAsync(IComponentContext scope)
        {
            //Инициализация Коллекции провайдеров
            var agrOption = scope.Resolve<IOptions<ProduserOptionAgregator>>();
            var produsersFactory = scope.Resolve<ProdusersFactory>();
            produsersFactory.FillProduserUnionByOptionAgregator(agrOption.Value);
            await Task.CompletedTask;
        }
    }
}
