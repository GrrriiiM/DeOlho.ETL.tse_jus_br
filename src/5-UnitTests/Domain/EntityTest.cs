using System.Linq;
using DeOlho.ETL.tse_jus_br.Domain.SeedWork;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace DeOlho.ETL.tse_jus_br.UnitTests.Domain
{
    public class EntityTest
    {
        [Fact]
        public void DomainEvents_Add_Clear()
        {
           var notificationMock = new Mock<INotification>();
           var entityMock = new Mock<Entity>();

           entityMock.Object.AddDomainEvent(notificationMock.Object);

           entityMock.Object.GetDomainEvents().Should().HaveCount(1);
           entityMock.Object.GetDomainEvents().ToList()[0].Should().Be(notificationMock.Object);
           entityMock.Object.ClearDomainEvents();
           entityMock.Object.GetDomainEvents().Should().HaveCount(0);

        }
    }
}