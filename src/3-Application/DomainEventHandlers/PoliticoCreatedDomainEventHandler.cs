using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Domain.Events;
using DeOlho.EventBus.TransferObjects.Messages.Services.Politicos;
using MediatR;
using RawRabbit;

namespace DeOlho.ETL.tse_jus_br.Application.DomainEventHandlers
{
    public class PublishIntegrationServiceWhenPoliticoCreatedDomainEventHandler : INotificationHandler<PoliticoCreatedDomainEvent>
    {

        readonly IBusClient _busClient;

        public PublishIntegrationServiceWhenPoliticoCreatedDomainEventHandler(
            IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task Handle(PoliticoCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var message = new PoliticoChangedMessage
            {
                Apelido = notification.Politico.NM_URNA_CANDIDATO,
                CPF = notification.Politico.NR_CPF_CANDIDATO.ToString(),
                Escolaridade = 1,
                Falecimento = null,
                MandatoFim = notification.Politico.DT_ELEICAO.AddYears(4),
                MandatoInicio = notification.Politico.DT_ELEICAO,
                MandatoSituacao = 1,
                Nascimento = notification.Politico.DT_NASCIMENTO,
                MandatoTipo = 1,
                NascimentoMunicipio = notification.Politico.NM_MUNICIPIO_NASCIMENTO,
                NascimentoUF = notification.Politico.SG_UF_NASCIMENTO,
                Nome = notification.Politico.NM_CANDIDATO,
                Partido = notification.Politico.SG_PARTIDO,
                Sexo = 1
            };

            await _busClient.PublishAsync(message);
        }

    }
}