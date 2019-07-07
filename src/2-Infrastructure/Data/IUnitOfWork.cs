using System.Threading;
using System.Threading.Tasks;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
    }
}