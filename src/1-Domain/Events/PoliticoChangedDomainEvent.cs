using System;
using MediatR;

namespace DeOlho.ETL.tse_jus_br.Domain.Events
{
    public class PoliticoChangedDomainEvent : INotification
    {
        public Politico Politico { get; protected set; }
        public PoliticoChangedDomainEvent(Politico politico)
        {
            Politico = politico;
        }
    }
}