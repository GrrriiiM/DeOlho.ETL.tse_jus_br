using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeOlho.ETL.Sources;
using DeOlho.ETL.Transforms;
using DeOlho.ETL.tse_jus_br.Domain;
using DeOlho.SeedWork.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeOlho.ETL.tse_jus_br.Application.Commands
{

    public class ExecuteETLPoliticoCommand : IRequest
    {
        public int? AnoEleicao { get; set; }
    }

    public class ExecuteETLPoliticoCommandHandler : IRequestHandler<ExecuteETLPoliticoCommand>
    {
        readonly ETLConfiguration _configuration;
        readonly IRepository<Politico> _politicoRepository;
        readonly IHttpClientFactory _httpClientFactory;

        public ExecuteETLPoliticoCommandHandler(
            IOptions<ETLConfiguration> configuration,
            IRepository<Politico> politicoRepository,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration.Value;
            _politicoRepository = politicoRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Unit> Handle(ExecuteETLPoliticoCommand request, CancellationToken cancellationToken)
        {
            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var anoEleicao = request.AnoEleicao ?? DateTime.Now.Year;

            var politicosUrl = string.Format(_configuration.PoliticosUrl, anoEleicao);
            var politicosNomeArquivos = _configuration.PoliticosNomeArquivos.Select(_ => string.Format(_, anoEleicao).ToUpper());

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Politico, Politico>()
                    .ForMember(_ => _.Id, _ => _.Ignore());
            });

            var mapper = config.CreateMapper();

            var politicos = _politicoRepository.Query.ToList().ToDictionary(_ => _.NR_CPF_CANDIDATO);

            var result = await new Process()
                .Extract(() => new HttpStreamSource(_httpClientFactory.CreateClient("deolho"), politicosUrl))
                .TransformDescompressStream()
                .TransformToList(_ => _.Value)
                .Where(_ => politicosNomeArquivos.Contains(_.Value.Name.ToUpper()))
                .Transform(_ => _.Value.Stream)
                .TransformCsvToDynamic(";")
                .TransformToList(_ => new List<dynamic>(_.Value))
                //.Where(_ => _.Value.CD_CARGO == "6" && (_.Value.CD_SIT_TOT_TURNO == "2" || _.Value.CD_SIT_TOT_TURNO == "3"))
                .AsParallel()
                .WithCancellation(cancellationToken)
                .WithDegreeOfParallelism(4)
                .Transform(_ => {
                    long cpf = Convert.ToInt64(_.Value.NR_CPF_CANDIDATO);
                    Politico politico = null;
                    if (politicos.ContainsKey(cpf)) politico = politicos[cpf];
                    var hasChange = false;
                    if (politico == null)
                    {
                        politico = new Politico(_.Value, mapper);
                        politicos.Add(cpf, politico);
                        _politicoRepository.Add(politico);
                        hasChange = true;
                    }
                    else
                    {
                        hasChange = politico.Update(_.Value, mapper);
                    }

                    //if (hasChange) _politicoRepository.UnityOfWork.SaveChanges();

                    return politico;
                })
                .Execute();    

                _politicoRepository.UnityOfWork.SaveChanges();
            
            return new Unit();
        }
    }
}