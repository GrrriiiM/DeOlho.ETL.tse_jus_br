using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DeOlho.ETL.Sources;
using DeOlho.ETL.Transforms;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.ETL.tse_jus_br.Infrastructure.Repositories;
using MediatR;

namespace DeOlho.ETL.tse_jus_br.Application.Commands
{
    public class ExecuteImportacaoETLCommand : IRequest
    {
        public int? AnoEleicao { get; set; }
    }

    public class ExecuteImportacaoETLCommandHandler : IRequestHandler<ExecuteImportacaoETLCommand>
    {
        readonly ETLConfiguration _configuration;
        readonly IImportacaoRepository _importacaoRepository;
        readonly IHttpClientFactory _httpClientFactory;

        public ExecuteImportacaoETLCommandHandler(
            ETLConfiguration configuration,
            IImportacaoRepository importacaoRepository,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _importacaoRepository = importacaoRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Unit> Handle(ExecuteImportacaoETLCommand request, CancellationToken cancellationToken)
        {
            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var anoEleicao = request.AnoEleicao ?? DateTime.Now.Year;

            var politicosUrl = string.Format(_configuration.PoliticosUrl, anoEleicao);
            var politicosNomeArquivos = _configuration.PoliticosNomeArquivos.Select(_ => string.Format(_, anoEleicao).ToUpper());

            var result = await new Process()
                .Extract(() => new HttpStreamSource(_httpClientFactory.CreateClient(), politicosUrl))
                .TransformDescompressStream()
                .TransformToList(_ => _.Value)
                .Where(_ => politicosNomeArquivos.Contains(_.Value.Name.ToUpper()))
                .Transform(_ => _.Value.Stream)
                .TransformCsvToDynamic(";")
                .Transform(_ => {
                    var strDataHoraGeracao = $"{_.Value.FirstOrDefault().DT_GERACAO} {_.Value.FirstOrDefault().HH_GERACAO}";
                    var dataHoraGeracao = DateTime.ParseExact(strDataHoraGeracao, "dd/MM/yyyy HH:mm:ss", cultureInfo);
                    var parent  = _.GetParent<StreamDescompressed>();
                    var importacao = new Importacao(dataHoraGeracao, politicosUrl, parent.Value.Name);
                    return new ImportacaoHelper(importacao, _.Value);
                })
                .TransformToList(_ => new List<dynamic>(_.Value.Registros))
                .Where(_ => _.Value.CD_CARGO == "6" && (_.Value.CD_SIT_TOT_TURNO == "2" || _.Value.CD_SIT_TOT_TURNO == "3"))
                .AsParallel()
                .WithDegreeOfParallelism(4)
                .Transform(_ => {
                    var actualPolitico = _importacaoRepository.FindByCPF(Convert.ToInt64(_.Value.NR_CPF_CANDIDATO));
                    var importacao = _.GetParent<ImportacaoHelper>().Value.Importacao;
                    if (actualPolitico == null)
                        importacao.AddNewPolitico(_.Value);
                    else
                        importacao.AddIfHasChangePolitico(_.Value, actualPolitico);
                    return importacao;
                })
                .Execute();
            
            var importacoes = result.Select(_ => _.Value).Distinct();

            await _importacaoRepository.AddRangeAsync(importacoes, cancellationToken);
            await _importacaoRepository.UnityOfWork.SaveChangesAsync(cancellationToken);

            return new Unit();
        }

        public class ImportacaoHelper
        {
            public ImportacaoHelper(
                Importacao importacao,
                IEnumerable<dynamic> registros)
            {
                Importacao = importacao;
                Registros = registros;
            }

            public Importacao Importacao { get; private set; }
            public IEnumerable<dynamic> Registros { get; private set; }
        }
    }
}