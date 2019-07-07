using System;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Data
{
    public class DeOlhoDbContext: DbContext, IUnitOfWork
    {

        private readonly IMediator _mediator;

        private DeOlhoDbContext(DbContextOptions<DeOlhoDbContext> options) : base(options) { }

        public DeOlhoDbContext(DbContextOptions<DeOlhoDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public override int SaveChanges()
        {
            _mediator.DispatchDomainEventsAsync(this).Wait();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _mediator.DispatchDomainEventsAsync(this).Wait();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Importacao>(build => {
                build.HasKey(_ => _.Id);
                build.Metadata.FindNavigation(nameof(Importacao.Politicos))
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Politico>(entity => {
                entity.HasKey(_ => _.Id);
                entity.HasIndex(_ => _.NR_CPF_CANDIDATO).IsUnique();
            });
        }
    }
}