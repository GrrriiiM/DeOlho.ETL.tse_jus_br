using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.ETL.tse_jus_br.Domain.Events;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;
using FluentAssertions;
using Xunit;

namespace DeOlho.ETL.tse_jus_br.UnitTests.Domain
{
    public class ImportacaoTest
    {

        ISingleObjectBuilder<Politico> politicoBuilder;
        ISingleObjectBuilder<Importacao> importacaoBuilder;
        private Importacao importacaoBuild()
        {
            return new Importacao(
                new DateTime(2019, 1,2,3,4,5),
                "http://teste.com.br", "teste.csv");
        }

        private dynamic convertToDynamic(Politico politico)
        {
            var registroImportacao = new ExpandoObject() as IDictionary<string, object>;
            
            foreach(var prop in politico.GetType().GetProperties())
            {
                if (prop.Name != nameof(politico.Importacao)
                    && prop.Name != nameof(politico.ImportacaoId))
                {
                    registroImportacao.Add(prop.Name, prop.GetValue(politico).ToString());
                }
            }

            return registroImportacao;
        }

        public ImportacaoTest()
        {
            BuilderSetup.ResetToDefaults();
            
            var namer = new RandomValuePropertyNamer(
                new RandomGenerator(),
                new ReflectionUtil(), 
                true,
                DateTime.Now, 
                DateTime.Now.AddDays(10), 
                true, 
                new BuilderSettings());

            BuilderSetup.SetPropertyNamerFor<Politico>(namer);
            BuilderSetup.DisablePropertyNamingFor<Politico, Importacao>(_ => _.Importacao);
            var random = new Random();
            politicoBuilder = Builder<Politico>.CreateNew()
                .With(_ => _.DT_ELEICAO, DateTime.Now.AddDays(random.NextDouble() + 100).Date) 
                .With(_ => _.DT_NASCIMENTO, DateTime.Now.AddDays(random.NextDouble() + 100).Date);

            importacaoBuilder = Builder<Importacao>.CreateNew();
        }

        [Fact]
        public void Constructor()
        {
            var importacao = importacaoBuild();

            importacao.DataHoraArquivo.Should().Be(new DateTime(2019, 1,2,3,4,5));
            importacao.UrlOrigem.Should().Be("http://teste.com.br");
            importacao.FileName.Should().Be("teste.csv");

            importacao.DataHoraImportacao.Should().BeMoreThan(new TimeSpan(DateTime.Now.AddSeconds(-30).Ticks));
            importacao.DataHoraImportacao.Should().BeLessThan(new TimeSpan(DateTime.Now.AddSeconds(30).Ticks));
        }


        [Fact]
        public void AddNewPolitico()
        {
            var politicoSetup  = politicoBuilder.Build();

            var importacao = importacaoBuild();

            

            var newPolitico = (Politico)importacao.AddNewPolitico(convertToDynamic(politicoSetup));

            newPolitico.Should().BeEquivalentTo(politicoSetup, 
                cfg => {
                    cfg.Excluding(_ => _.Id);
                    cfg.Excluding(_ => _.Importacao);
                    cfg.Excluding(_ => _.ImportacaoId);
                    return cfg;
                });

            importacao.Politicos.Should().HaveCount(1);
            importacao.Politicos.ToArray()[0].Should().BeEquivalentTo(newPolitico);

            importacao.GetDomainEvents().Should().HaveCount(1);
            importacao.GetDomainEvents().ToArray()[0].Should().BeAssignableTo(typeof(PoliticoCreatedDomainEvent));
            ((PoliticoCreatedDomainEvent)importacao.GetDomainEvents().ToArray()[0]).Politico.Should().BeEquivalentTo(newPolitico);

        }

        [Fact]
        public void AddIfHasChangePolitico_NotHasChange()
        {
            var importacao = importacaoBuild();

            var politico = politicoBuilder.Build();

            var hasChange = (bool)importacao.AddIfHasChangePolitico(convertToDynamic(politico), politico);

            hasChange.Should().Be(false);

            importacao.Politicos.Should().HaveCount(0);

            importacao.GetDomainEvents().Should().HaveCount(0);

        }

        [Fact]
        public void AddIfHasChangePolitico_HasChange()
        {
            var importacao = importacaoBuild();

            var politico1 = politicoBuilder.Build();
            var politico2 = politicoBuilder.Build();

            var hasChange = (bool)importacao.AddIfHasChangePolitico(convertToDynamic(politico1), politico2);
            
            hasChange.Should().Be(true);

            importacao.Politicos.Should().HaveCount(1);

            var newPolitico = importacao.Politicos.ToList()[0];
            
            newPolitico.ImportacaoId.Should().Be(importacao.Id);
            newPolitico.Importacao.Should().Be(importacao);
            newPolitico.Id.Should().Be(politico2.Id);

            newPolitico.Should().BeEquivalentTo(politico2, 
                cfg => {
                    cfg.Excluding(_ => _.Importacao);
                    cfg.Excluding(_ => _.ImportacaoId);
                    return cfg;
                });

            importacao.GetDomainEvents().Should().HaveCount(1);
            importacao.GetDomainEvents().ToArray()[0].Should().BeAssignableTo(typeof(PoliticoChangedDomainEvent));
            ((PoliticoChangedDomainEvent)importacao.GetDomainEvents().ToArray()[0]).Politico.Should().BeEquivalentTo(newPolitico);
        }

    }
}