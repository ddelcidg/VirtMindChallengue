using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Host.Controllers
{
   // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly CurrencyExchangeService _currencyExchangeService;

        public CurrencyExchangeController(CurrencyExchangeService currencyExchangeService)
        {
            _currencyExchangeService = currencyExchangeService;
        }

        //[HttpPost("purchase")]
        [HttpPost(Name = "purchase")]
        public IActionResult PurchaseCurrency([FromBody] CurrencyPurchaseRequest request)
        {
            try
            {
                // Validate the request
                var validationResult = _currencyExchangeService.ValidatePurchaseRequest(request);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ErrorMessage);
                }

                // Perform the currency purchase
                var result = _currencyExchangeService.PurchaseCurrency(request);

                // Return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the error
                // Implement proper error handling/logging here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("transactions")]
        public IActionResult GetTransactions([FromQuery] string userId)
        {
            try
            {
                // Query transactions based on the user ID using the service layer
                var transactions = _currencyExchangeService.GetTransactionsByUserId(userId);

                // Return the transactions
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                // Log the error
                // Implement proper error handling/logging here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }

    public class CurrencyPurchaseRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
