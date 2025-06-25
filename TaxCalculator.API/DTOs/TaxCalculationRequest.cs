using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.API.DTOs
{
    public class TaxCalculationRequest
    {
        [Range(1, int.MaxValue / 2, ErrorMessage = "The value must be greater than 0.")]
        public int AnnualSalary { get; set; }
    }
}
