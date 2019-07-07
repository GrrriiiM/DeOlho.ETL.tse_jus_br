using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Application;
using DeOlho.ETL.tse_jus_br.Domain.SeedWork;
using DeOlho.ETL.tse_jus_br.Infrastructure.Data;
using DeOlho.ETL.tse_jus_br.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RawRabbit.vNext;
using Swashbuckle.AspNetCore.Swagger;

namespace DeOlho.ETL.tse_jus_br.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DeOlhoDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ETL"));
            });

            services.AddTransient<ETLConfiguration>(_ => Configuration.GetSection("ETL:Configuration").Get<ETLConfiguration>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DeOlho tse_jus_br", Version = "v1" });
            });

            services.AddRawRabbit(
                config => config.AddConfiguration(Configuration.GetSection("RawRabbit:Configuration")),
                custom => { }
            );

            services.AddTransient<IImportacaoRepository, ImportacaoRepository>();
            services.AddHttpClient();

            services.AddMediatR(
                typeof(Startup),
                typeof(ETLConfiguration),
                typeof(DeOlhoDbContext),
                typeof(Entity));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeOlho tse_jus_br API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMigrate();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
