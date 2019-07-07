using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Infrastructure.Data;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        IUnitOfWork UnityOfWork { get; }

        T Add(T entity);
        void AddRange(IEnumerable<T> entities);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));
        T Find(object key);
        Task<T> FindAsync(object key, CancellationToken cancellationToken = default(CancellationToken));
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        T Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
    }
}