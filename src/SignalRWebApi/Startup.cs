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



        private async Task InitializeAsync(ILifetimeScope scope)
        {
            //DEBUG
            var agrOption= new ProduserOptionAgregator
            {
                KafkaProduserOptions = new List<KafkaProduserOption>
                {
                    new KafkaProduserOption
                    {
                        Key = "Kafka_1",
                        TimeRequest = TimeSpan.FromSeconds(3),
                        TrottlingQuantity = 10,
                        BrokerEndpoints = "192.168.100.3",
                        TopicName = "ReceiveMessage"
                    }
                },
                SignalRProduserOptions = new List<SignalRProduserOption>
                {
                    new SignalRProduserOption
                    {
                        Key = "signalR_1",
                        TimeRequest = TimeSpan.FromSeconds(3),
                        TrottlingQuantity = 10,
                        MethodeName = "ReceiveMessage"
                    }
                }
            };
             //Заполнение сервиса
            var produsersFactory = scope.Resolve<ProdusersFactory>();
            produsersFactory.FillProduserUnionByOptionAgregator(agrOption);
            await Task.CompletedTask;
        }
    }
}
