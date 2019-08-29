using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeOlho.SeedWork;
using MediatR;
using System.Reflection;
using DeOlho.SeedWork.Infrastructure.Data;
using DeOlho.SeedWork.Domain.Abstractions;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.SeedWork.Infrastructure.Repositories;
using DeOlho.ETL.tse_jus_br.Application;
using DeOlho.EventBus.RabbitMQ;
using DeOlho.EventBus;

namespace DeOlho.ETL.tse_jus_br
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
            services.AddSeedWork(new DeOlhoDbContextConfiguration(
                Configuration.GetConnectionString("DeOlho"),
                this.GetType().Assembly,
                this.GetType().Assembly));

            services.Configure<ETLConfiguration>(options => Configuration.GetSection("ETLConfiguration").Bind(options));

            services.AddScoped<IRepository<Politico>, Repository<Politico>>();

            services.AddScoped<PoliticoAutoMapper>();

            services.AddEventBusRabbitMQ(c => 
            {
                c.Configuration(Configuration.GetSection("EventBus"));
            });

            services.AddMediatR(Assembly.GetEntryAssembly());

            services.AddHostedService<IntegrationEventBackgroundService>();

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

            app.UseSeedWork();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
