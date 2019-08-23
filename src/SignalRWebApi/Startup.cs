using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Enums;
using AbstractProduser.Options;
using Autofac;
using Exchange.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProdusersMediator;
using WebApi.AutofacModules;
using WebApi.Hubs;
using WebClientProduser;

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
            services.Configure<ProduserUnionOption>(Configuration);
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

            services.AddHttpClient<IHttpClientSupport, HttpClientSupport>();
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
            var agrOption = scope.Resolve<IOptions<ProduserUnionOption>>();

            //вариант для DI где produseUnion singleton.
            //var produsersFactory = scope.Resolve<ProdusersFactory>();
            //produsersFactory.FillProduserUnionByOptionAgregator(agrOption.Value);

            //вариант для CWS
            var produserUnionFactory = scope.Resolve<ProdusersUnionFactory<AdInputTypeFake>>(); //регистрируем и также резолвим в зависимости от типа входа (AdInputTypeFake).
            var produserUnion = produserUnionFactory.FillProduserUnionByOptionAgregator(agrOption.Value);
            //produserUnion добавить в Storage по ключу

            await Task.CompletedTask;
        }
    }
}
