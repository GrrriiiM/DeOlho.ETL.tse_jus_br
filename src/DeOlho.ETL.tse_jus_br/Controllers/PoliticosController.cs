using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeOlho.ETL.tse_jus_br.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeOlho.ETL.tse_jus_br.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliticosController : ControllerBase
    {
        readonly IMediator _mediator;
        public PoliticosController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("ETL")]
        public Task ExecuteETL([FromBody]ExecuteETLPoliticoCommand command)
        {
            return _mediator.Send(command);
        }
    }
}
