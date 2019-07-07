using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeOlho.ETL.tse_jus_br.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Importacao",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraImportacao = table.Column<DateTime>(nullable: false),
                    DataHoraArquivo = table.Column<DateTime>(nullable: true),
                    UrlOrigem = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Importacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Politico",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImportacaoId = table.Column<long>(nullable: false),
                    ANO_ELEICAO = table.Column<int>(nullable: false),
                    CD_TIPO_ELEICAO = table.Column<int>(nullable: false),
                    NM_TIPO_ELEICAO = table.Column<string>(nullable: true),
                    NR_TURNO = table.Column<int>(nullable: false),
                    CD_ELEICAO = table.Column<int>(nullable: false),
                    DS_ELEICAO = table.Column<string>(nullable: true),
                    DT_ELEICAO = table.Column<DateTime>(nullable: false),
                    TP_ABRANGENCIA = table.Column<string>(nullable: true),
                    SG_UF = table.Column<string>(nullable: true),
                    SG_UE = table.Column<string>(nullable: true),
                    NM_UE = table.Column<string>(nullable: true),
                    CD_CARGO = table.Column<int>(nullable: false),
                    DS_CARGO = table.Column<string>(nullable: true),
                    SQ_CANDIDATO = table.Column<long>(nullable: false),
                    NR_CANDIDATO = table.Column<int>(nullable: false),
                    NM_CANDIDATO = table.Column<string>(nullable: true),
                    NM_URNA_CANDIDATO = table.Column<string>(nullable: true),
                    NM_SOCIAL_CANDIDATO = table.Column<string>(nullable: true),
                    NR_CPF_CANDIDATO = table.Column<long>(nullable: false),
                    NM_EMAIL = table.Column<string>(nullable: true),
                    CD_SITUACAO_CANDIDATURA = table.Column<int>(nullable: false),
                    DS_SITUACAO_CANDIDATURA = table.Column<string>(nullable: true),
                    CD_DETALHE_SITUACAO_CAND = table.Column<int>(nullable: false),
                    DS_DETALHE_SITUACAO_CAND = table.Column<string>(nullable: true),
                    TP_AGREMIACAO = table.Column<string>(nullable: true),
                    NR_PARTIDO = table.Column<int>(nullable: false),
                    SG_PARTIDO = table.Column<string>(nullable: true),
                    NM_PARTIDO = table.Column<string>(nullable: true),
                    SQ_COLIGACAO = table.Column<long>(nullable: false),
                    NM_COLIGACAO = table.Column<string>(nullable: true),
                    DS_COMPOSICAO_COLIGACAO = table.Column<string>(nullable: true),
                    CD_NACIONALIDADE = table.Column<int>(nullable: false),
                    DS_NACIONALIDADE = table.Column<string>(nullable: true),
                    SG_UF_NASCIMENTO = table.Column<string>(nullable: true),
                    CD_MUNICIPIO_NASCIMENTO = table.Column<int>(nullable: false),
                    NM_MUNICIPIO_NASCIMENTO = table.Column<string>(nullable: true),
                    DT_NASCIMENTO = table.Column<DateTime>(nullable: false),
                    NR_IDADE_DATA_POSSE = table.Column<int>(nullable: false),
                    NR_TITULO_ELEITORAL_CANDIDATO = table.Column<long>(nullable: false),
                    CD_GENERO = table.Column<int>(nullable: false),
                    DS_GENERO = table.Column<string>(nullable: true),
                    CD_GRAU_INSTRUCAO = table.Column<int>(nullable: false),
                    DS_GRAU_INSTRUCAO = table.Column<string>(nullable: true),
                    CD_ESTADO_CIVIL = table.Column<int>(nullable: false),
                    DS_ESTADO_CIVIL = table.Column<string>(nullable: true),
                    CD_COR_RACA = table.Column<int>(nullable: false),
                    DS_COR_RACA = table.Column<string>(nullable: true),
                    CD_OCUPACAO = table.Column<int>(nullable: false),
                    DS_OCUPACAO = table.Column<string>(nullable: true),
                    NR_DESPESA_MAX_CAMPANHA = table.Column<int>(nullable: false),
                    CD_SIT_TOT_TURNO = table.Column<int>(nullable: false),
                    DS_SIT_TOT_TURNO = table.Column<string>(nullable: true),
                    ST_REELEICAO = table.Column<string>(nullable: true),
                    ST_DECLARAR_BENS = table.Column<string>(nullable: true),
                    NR_PROTOCOLO_CANDIDATURA = table.Column<int>(nullable: false),
                    NR_PROCESSO = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Politico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Politico_Importacao_ImportacaoId",
                        column: x => x.ImportacaoId,
                        principalTable: "Importacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Politico_ImportacaoId",
                table: "Politico",
                column: "ImportacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Politico_NR_CPF_CANDIDATO",
                table: "Politico",
                column: "NR_CPF_CANDIDATO",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Politico");

            migrationBuilder.DropTable(
                name: "Importacao");
        }
    }
}
