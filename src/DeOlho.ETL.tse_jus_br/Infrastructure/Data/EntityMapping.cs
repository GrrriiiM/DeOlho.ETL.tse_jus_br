using DeOlho.ETL.tse_jus_br.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Data
{
    public class PoliticoEntityMapping : IEntityTypeConfiguration<Politico>
    {
        public void Configure(EntityTypeBuilder<Politico> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasIndex(_ => _.NR_CPF_CANDIDATO).IsUnique();
        }
    }
}