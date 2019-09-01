using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.ETL.tse_jus_br.Messages;
using DeOlho.EventBus.Abstractions;
using DeOlho.SeedWork.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeOlho.ETL.tse_jus_br
{
    public class IntegrationEventBackgroundService : BackgroundService
    {
        PoliticoAutoMapper _politicoAutoMapper;
        DeOlhoDbContext _deOlhoDbContext;
        readonly IServiceProvider _serviceProvider;
        readonly IEventBus _eventBus;

        public IntegrationEventBackgroundService(
            IServiceProvider serviceProvider,
            IEventBus eventBus)
        {
            _serviceProvider = serviceProvider;
            _eventBus = eventBus;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
                
            while(!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    _deOlhoDbContext = scope.ServiceProvider.GetService<DeOlhoDbContext>();
                    _politicoAutoMapper = scope.ServiceProvider.GetService<PoliticoAutoMapper>();
                
                    var messagePoliticos = new List<PoliticoChangedMessage.Politico>();
                    foreach(var politico in _deOlhoDbContext.Set<Politico>().Where(_ => !_.Publicado).Take(1000).ToList())
                    {
                        var messagePolitico = new PoliticoChangedMessage.Politico();
                        _politicoAutoMapper.MapToChangedMessage(politico, messagePolitico);

                        politico.Publicado = true;
                        politico.Erro = false;
                        politico.DescricaoErro = null;

                        messagePoliticos.Add(messagePolitico);
                    }

                    if (messagePoliticos.Any())
                    {
                        _eventBus.Publish<PoliticoChangedMessage>(new PoliticoChangedMessage(Guid.NewGuid().ToString(), messagePoliticos));
                        await _deOlhoDbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}