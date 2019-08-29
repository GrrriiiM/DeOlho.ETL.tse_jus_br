using AutoMapper;
using DeOlho.ETL.tse_jus_br.Messages;

namespace DeOlho.ETL.tse_jus_br.Domain
{
    public class PoliticoAutoMapper
    {
        readonly IMapper _mapper;
        public PoliticoAutoMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Politico, Politico>()
                    .ForMember(_ => _.Id, _ => _.Ignore());
                cfg.CreateMap<Politico, PoliticoChangedMessage>();
            });
            _mapper = config.CreateMapper();
        }

        public Politico MapFromRegistroImportacao(dynamic registroImportacao)
        {
            return _mapper.Map<Politico>(registroImportacao);
        }

        public Politico MapFromRegistroImportacao(dynamic registroImportacao, Politico politico)
        {
            return _mapper.Map(registroImportacao, politico);
        }

        public Politico MapFromPolitico(Politico politicoEntrada, Politico politicoSaida)
        {
            return _mapper.Map(politicoEntrada, politicoSaida);
        }

        public PoliticoChangedMessage MapToChangedMessage(Politico politico, PoliticoChangedMessage politicoChangedMessage)
        {
            return _mapper.Map(politico, politicoChangedMessage);
        }
    }
}