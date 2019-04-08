using System;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.OwnedInstances;
using KafkaProduser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProdusersUnion;
using WebApi.AutofacModules;
using WebApi.Hubs;
using WebApi.Produsers;

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
                routes.MapHub<BaseHub>("/baseHub");
            });
            app.UseMvc();
        }



        private async Task InitializeAsync(ILifetimeScope scope)
        {
            var prodUnionServ = scope.Resolve<ProdusersUnionService>();

            IProduser kaffkaProd1 = new KafkaProduserWrapper(TimeSpan.FromSeconds(3), 10, new KaffkaProduserOption { Key = "Kafka1", BrokerEndpoints = "" });
            IProduser kaffkaProd2 = new KafkaProduserWrapper(TimeSpan.FromSeconds(0.5), 10, new KaffkaProduserOption { Key = "Kafka2", BrokerEndpoints = "" });
            var signalRProdFactory = scope.Resolve<Func<TimeSpan, int, Owned<SignalRProduser>>>(); //TODO: передавать Option
            var produserOwner = signalRProdFactory(TimeSpan.FromSeconds(3),10);
            var signalRProd = produserOwner.Value;

            prodUnionServ.AddProduser("kaffka1", kaffkaProd1);
            prodUnionServ.AddProduser("kaffka2", kaffkaProd2);
            prodUnionServ.AddProduser("signalRProd1", signalRProd);

            await Task.CompletedTask;
        }
    }
}
