using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaxCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TaxController> _logger;

        public TaxController(IMediator mediator, ILogger<TaxController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
    }
}
