using System;
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
            using(var scope = _serviceProvider.CreateScope())
            {
                _deOlhoDbContext = scope.ServiceProvider.GetService<DeOlhoDbContext>();
                _politicoAutoMapper = scope.ServiceProvider.GetService<PoliticoAutoMapper>();
                
                while(!stoppingToken.IsCancellationRequested)
                {
                    foreach(var politico in _deOlhoDbContext.Set<Politico>().Where(_ => !_.Published).Take(1000).ToList())
                    {
                        var message  =  new PoliticoChangedMessage(Guid.NewGuid().ToString());

                        _politicoAutoMapper.MapToChangedMessage(politico, message);

                        _eventBus.Publish<PoliticoChangedMessage>(message);

                        politico.Published = true;

                        await _deOlhoDbContext.SaveChangesAsync(stoppingToken);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }
    }
}