using System;
using System.Collections.Generic;
using AutoMapper;
using DeOlho.ETL.tse_jus_br.Domain.Events;
using DeOlho.ETL.tse_jus_br.Domain.SeedWork;

namespace DeOlho.ETL.tse_jus_br.Domain
{
    public class Importacao : Entity
    {


        private IMapper _mapper;

        public DateTime DataHoraImportacao { get; protected set; }
        public DateTime? DataHoraArquivo { get; protected set; }
        public string UrlOrigem { get; protected set; }
        public string FileName { get; protected set; }

        private List<Politico> _politicos = new List<Politico>();
        public IReadOnlyCollection<Politico> Politicos => _politicos.AsReadOnly();

        private Importacao() {}

        public Importacao(DateTime dataHoraArquivo, string urlOrigem, string fileName)
        {
            DataHoraArquivo = dataHoraArquivo;
            UrlOrigem = urlOrigem;
            FileName = fileName;
            DataHoraImportacao = DateTime.Now;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Politico, Politico>()
                    .ForMember(_ => _.Id, _ => _.Ignore());
            });

            _mapper = config.CreateMapper();
        
        }

        public Politico AddNewPolitico(dynamic registroImportacao)
        {
            var politico = (Politico)_mapper.Map<Politico>(registroImportacao);
            _politicos.Add(politico);
            AddDomainEvent(new PoliticoCreatedDomainEvent(politico));
            return politico;
        }

        public bool AddIfHasChangePolitico(dynamic registroImportacao, Politico actualPolitico)
        {
            var politico = (Politico)_mapper.Map<Politico>(registroImportacao);
            var hasChange = !PropertyCompare.Equal(politico, actualPolitico, 
                nameof(politico.Id),
                nameof(politico.ImportacaoId),
                nameof(politico.Importacao));
            if (hasChange)
            {
                actualPolitico = _mapper.Map<Politico, Politico>(politico, actualPolitico);
                actualPolitico.SetImportacao(this);
                _politicos.Add(actualPolitico);
                AddDomainEvent(new PoliticoChangedDomainEvent(actualPolitico));
            }
            return hasChange;
        }
    }
}