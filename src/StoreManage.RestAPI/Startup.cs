using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StoreManage.Infrastructure.Application;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Services.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManage.RestAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreManage.RestAPI", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<EFDataContext>()
                .WithParameter("connectionString", Configuration["ConnectionString"])
                 .AsSelf()
                 .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(EFCommodityRepository).Assembly)
                      .AssignableTo<Repository>()
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CommodityAppService).Assembly)
                      .AssignableTo<Service>()
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterType<EFUnitOfWork>()
                .As<UnitOfWork>()
                .InstancePerLifetimeScope();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreManage.RestAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
