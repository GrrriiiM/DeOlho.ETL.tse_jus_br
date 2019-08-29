using System;
using DeOlho.EventBus.Message;

namespace DeOlho.ETL.tse_jus_br.Messages
{
    public class PoliticoChangedMessage : EventBusMessage
    {
        public PoliticoChangedMessage(string messageId) : base(messageId)
        {
        }

        public int ANO_ELEICAO { get; set; }
        public int CD_TIPO_ELEICAO { get; set; }
        public string NM_TIPO_ELEICAO { get; set; }
        public int NR_TURNO { get; set; }
        public int CD_ELEICAO { get; set; }
        public string DS_ELEICAO { get; set; }
        public DateTime DT_ELEICAO { get; set; }
        public string TP_ABRANGENCIA { get; set; }
        public string SG_UF { get; set; }
        public string SG_UE { get; set; }
        public string NM_UE { get; set; }
        public int CD_CARGO { get; set; }
        public string DS_CARGO { get; set; }
        public long SQ_CANDIDATO { get; set; }
        public int NR_CANDIDATO { get; set; }
        public string NM_CANDIDATO { get; set; }
        public string NM_URNA_CANDIDATO { get; set; }
        public string NM_SOCIAL_CANDIDATO { get; set; }
        public long NR_CPF_CANDIDATO { get; set; }
        public string NM_EMAIL { get; set; }
        public int CD_SITUACAO_CANDIDATURA { get; set; }
        public string DS_SITUACAO_CANDIDATURA { get; set; }
        public int CD_DETALHE_SITUACAO_CAND { get; set; }
        public string DS_DETALHE_SITUACAO_CAND { get; set; }
        public string TP_AGREMIACAO { get; set; }
        public int NR_PARTIDO { get; set; }
        public string SG_PARTIDO { get; set; }
        public string NM_PARTIDO { get; set; }
        public long SQ_COLIGACAO { get; set; }
        public string NM_COLIGACAO { get; set; }
        public string DS_COMPOSICAO_COLIGACAO { get; set; }
        public int CD_NACIONALIDADE { get; set; }
        public string DS_NACIONALIDADE { get; set; }
        public string SG_UF_NASCIMENTO { get; set; }
        public int CD_MUNICIPIO_NASCIMENTO { get; set; }
        public string NM_MUNICIPIO_NASCIMENTO { get; set; }
        public DateTime DT_NASCIMENTO { get; set; }
        public int NR_IDADE_DATA_POSSE { get; set; }
        public long NR_TITULO_ELEITORAL_CANDIDATO { get; set; }
        public int CD_GENERO { get; set; }
        public string DS_GENERO { get; set; }
        public int CD_GRAU_INSTRUCAO { get; set; }
        public string DS_GRAU_INSTRUCAO { get; set; }
        public int CD_ESTADO_CIVIL { get; set; }
        public string DS_ESTADO_CIVIL { get; set; }
        public int CD_COR_RACA { get; set; }
        public string DS_COR_RACA { get; set; }
        public int CD_OCUPACAO { get; set; }
        public string DS_OCUPACAO { get; set; }
        public int NR_DESPESA_MAX_CAMPANHA { get; set; }
        public int CD_SIT_TOT_TURNO { get; set; }
        public string DS_SIT_TOT_TURNO { get; set; }
        public string ST_REELEICAO { get; set; }
        public string ST_DECLARAR_BENS { get; set; }
        public int NR_PROTOCOLO_CANDIDATURA { get; set; }
        public long NR_PROCESSO { get; set; }

    }
}
