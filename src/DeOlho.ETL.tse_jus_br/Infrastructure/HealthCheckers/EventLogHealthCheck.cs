using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.SeedWork.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.HealthCheckers
{
    public class EventLogHealthCheck : IHealthCheck
    {
        readonly IRepository<Politico> _politicoRepository;

        public EventLogHealthCheck(
            IRepository<Politico> politicoRepository)
        {
            _politicoRepository = politicoRepository;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (await _politicoRepository.Query.AnyAsync(_ => !_.Publicado, cancellationToken))
            {
                return HealthCheckResult.Degraded();
            }
            else if (await _politicoRepository.Query.AnyAsync(_ => _.Erro, cancellationToken))
            {
                return HealthCheckResult.Unhealthy();
            }
            else
            {
                return HealthCheckResult.Healthy();
            }
        }
    }
}