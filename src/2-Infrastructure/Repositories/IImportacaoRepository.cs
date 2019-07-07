using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.ETL.tse_jus_br.Infrastructure.Repositories;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Repositories
{
    public interface IImportacaoRepository : IRepository<Importacao>
    {
        Task<Politico> FindByCPFAsync(long cpf);
        Politico FindByCPF(long cpf);
    }
}