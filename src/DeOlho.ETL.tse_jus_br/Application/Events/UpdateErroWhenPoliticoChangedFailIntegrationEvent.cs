using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Messages;
using DeOlho.EventBus.MediatR;
using DeOlho.SeedWork.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeOlho.ETL.tse_jus_br.Application.Events
{
    public class UpdateErroWhenPoliticoChangedFailIntegrationEvent : EventBusConsumerFailHandler<PoliticoChangedMessage>
    {
        readonly DeOlhoDbContext _deOlhoDbContext;
        //readonly IRepository<Domain.Politico> _politicoRepository;

        public UpdateErroWhenPoliticoChangedFailIntegrationEvent(
            DeOlhoDbContext deOlhoDbContext)
            //IRepository<Domain.Politico> politicoRepository)
        {
            _deOlhoDbContext = deOlhoDbContext;   
            //_politicoRepository = politicoRepository;
        }

        public async override Task<Unit> Handle(PoliticoChangedMessage message, string[] exceptionStack, CancellationToken cancellationToken)
        {
            var politicoRepository = _deOlhoDbContext.Set<Domain.Politico>();
            var id = long.Parse(message.MessageId);
            var politico = await politicoRepository.SingleOrDefaultAsync(_ => _.Id == id);
            if (politico != null)
            {
                politico.Erro = true;
                politico.DescricaoErro = exceptionStack[0];
            }

            return Unit.Value;
        }
        
    }
}