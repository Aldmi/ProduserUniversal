using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRWebApiProduser.Hubs;

namespace SignalRWebApiProduser
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IContainer ApplicationContainer { get; private set; }



        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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



            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);       
        }




        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            //app.UseMvc();
        }
    }
}
