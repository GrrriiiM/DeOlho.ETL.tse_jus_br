using System;
using MediatR;

namespace DeOlho.ETL.tse_jus_br.Domain.Events
{
    public class PoliticoCreatedDomainEvent : INotification
    {
        public Politico Politico { get; protected set; }
        public PoliticoCreatedDomainEvent(Politico politico)
        {
            Politico = politico;
        }
    }
}