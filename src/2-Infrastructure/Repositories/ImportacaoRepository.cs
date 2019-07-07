using System.Linq;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.ETL.tse_jus_br.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Repositories
{
    public class ImportacaoRepository : Repository<Importacao>, IImportacaoRepository
    {
        public ImportacaoRepository(
            DeOlhoDbContext deOlhoDbContext)
            : base(deOlhoDbContext)
        {
            
        }

        public async Task<Politico> FindByCPFAsync(long cpf)
        {
            return await Query.SelectMany(_ => _.Politicos).SingleOrDefaultAsync(_ => _.NR_CPF_CANDIDATO == cpf);
        }

        public Politico FindByCPF(long cpf)
        {
            return Query.SelectMany(_ => _.Politicos).SingleOrDefault(_ => _.NR_CPF_CANDIDATO == cpf);
        }
    }
}