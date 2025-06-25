using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaxCalculator.API.DTOs;
using TaxCalculator.Application.Commands;
using TaxCalculator.Application.Queries;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly IMediator _mediator;       

        public TaxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Calculates tax based on gross annual salary.
        /// If calculation is successfull saves into the Database
        /// </summary>
        /// <param name="annualSalary"></param>
        /// <returns></returns>
        [HttpPost("calculate")]
        public async Task<ActionResult<TaxResult>> CalculateTax(
           [FromBody] TaxCalculationRequest request)
        {
            var taxResult = await _mediator.Send(new CalculateTaxCommand { AnnualSalary = request.AnnualSalary });
            if (taxResult != null)
            {
                await _mediator.Send(new SaveTaxResultCommand { TaxResult = taxResult });
            }

            return Ok(taxResult);
        }

        /// <summary>
        /// Retrieves the history of calculated tax results, sorted by CreatedDate.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<ActionResult<List<TaxResult>>> GetTaxCalculationHistory(
            [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
            [FromQuery] int amount = 10)
        {         
            var taxResults = await _mediator.Send(new RetrieveTaxResultsQuery { Amount = amount });
            return Ok(taxResults);
        }
    }
}
