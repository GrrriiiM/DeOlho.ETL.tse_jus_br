using System;
using DeOlho.ETL.tse_jus_br.Domain.Events;
using DeOlho.ETL.tse_jus_br.Domain.SeedWork;

namespace DeOlho.ETL.tse_jus_br.Domain
{
    public class Politico : Entity
    {
        internal Politico() {}
        
        // internal Politico(Importacao importacao, dynamic registroImportacao)
        // {
        //     this.Importacao = importacao;
        //     this.ImportacaoId = importacao.Id;

        //     this.setValues(registroImportacao);

        //     AddDomainEvent(new PoliticoCreatedDomainEvent(this));
        // }

        public Importacao Importacao {get; protected set;}
        public long ImportacaoId {get; protected set; }

        public int ANO_ELEICAO { get; protected set; }
        public int CD_TIPO_ELEICAO { get; protected set; }
        public string NM_TIPO_ELEICAO { get; protected set; }
        public int NR_TURNO { get; protected set; }
        public int CD_ELEICAO { get; protected set; }
        public string DS_ELEICAO { get; protected set; }
        public DateTime DT_ELEICAO { get; protected set; }
        public string TP_ABRANGENCIA { get; protected set; }
        public string SG_UF { get; protected set; }
        public string SG_UE { get; protected set; }
        public string NM_UE { get; protected set; }
        public int CD_CARGO { get; protected set; }
        public string DS_CARGO { get; protected set; }
        public long SQ_CANDIDATO { get; protected set; }
        public int NR_CANDIDATO { get; protected set; }
        public string NM_CANDIDATO { get; protected set; }
        public string NM_URNA_CANDIDATO { get; protected set; }
        public string NM_SOCIAL_CANDIDATO { get; protected set; }
        public long NR_CPF_CANDIDATO { get; protected set; }
        public string NM_EMAIL { get; protected set; }
        public int CD_SITUACAO_CANDIDATURA { get; protected set; }
        public string DS_SITUACAO_CANDIDATURA { get; protected set; }
        public int CD_DETALHE_SITUACAO_CAND { get; protected set; }
        public string DS_DETALHE_SITUACAO_CAND { get; protected set; }
        public string TP_AGREMIACAO { get; protected set; }
        public int NR_PARTIDO { get; protected set; }
        public string SG_PARTIDO { get; protected set; }
        public string NM_PARTIDO { get; protected set; }
        public long SQ_COLIGACAO { get; protected set; }
        public string NM_COLIGACAO { get; protected set; }
        public string DS_COMPOSICAO_COLIGACAO { get; protected set; }
        public int CD_NACIONALIDADE { get; protected set; }
        public string DS_NACIONALIDADE { get; protected set; }
        public string SG_UF_NASCIMENTO { get; protected set; }
        public int CD_MUNICIPIO_NASCIMENTO { get; protected set; }
        public string NM_MUNICIPIO_NASCIMENTO { get; protected set; }
        public DateTime DT_NASCIMENTO { get; protected set; }
        public int NR_IDADE_DATA_POSSE { get; protected set; }
        public long NR_TITULO_ELEITORAL_CANDIDATO { get; protected set; }
        public int CD_GENERO { get; protected set; }
        public string DS_GENERO { get; protected set; }
        public int CD_GRAU_INSTRUCAO { get; protected set; }
        public string DS_GRAU_INSTRUCAO { get; protected set; }
        public int CD_ESTADO_CIVIL { get; protected set; }
        public string DS_ESTADO_CIVIL { get; protected set; }
        public int CD_COR_RACA { get; protected set; }
        public string DS_COR_RACA { get; protected set; }
        public int CD_OCUPACAO { get; protected set; }
        public string DS_OCUPACAO { get; protected set; }
        public int NR_DESPESA_MAX_CAMPANHA { get; protected set; }
        public int CD_SIT_TOT_TURNO { get; protected set; }
        public string DS_SIT_TOT_TURNO { get; protected set; }
        public string ST_REELEICAO { get; protected set; }
        public string ST_DECLARAR_BENS { get; protected set; }
        public int NR_PROTOCOLO_CANDIDATURA { get; protected set; }
        public long NR_PROCESSO { get; protected set; }

        internal void SetImportacao(Importacao importacao)
        {
            Importacao = importacao;
            ImportacaoId = importacao.Id;
        }
    }
}