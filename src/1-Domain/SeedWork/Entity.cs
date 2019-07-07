using System.Collections.Generic;
using MediatR;

namespace DeOlho.ETL.tse_jus_br.Domain.SeedWork
{
    public class Entity
    {
        protected Entity() { }
        public long Id { get; set; }
        private List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> GetDomainEvents() => _domainEvents?.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

    }
}