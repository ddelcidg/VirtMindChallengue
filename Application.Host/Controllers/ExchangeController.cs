using Application.Host.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Application.Host.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeController : Controller
    {
        /*private readonly ILogger<ExchangeController> _logger;
        private readonly ExchangeService _exchangeService;
        //1- Domain, CrossCore
        public ExchangeController(ILogger<ExchangeController> logger, ExchangeService exchangeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
        }*/

        private readonly ILogger<ExchangeController> _logger;
        private readonly IExchangeService _exchangeService;

        public ExchangeController(ILogger<ExchangeController> logger, IExchangeService exchangeService)
        {
            _logger = logger;
            _exchangeService = exchangeService;
        }
        [HttpPost(Name = "get-by-currenry")]
        public async Task<IActionResult> GetByCurrency(string currency)
        {
            ValidateCurrency(currency);
            //var values = await GetValues();
            var values = await _exchangeService.GetValuesFromExternalAPI();
            var exchangeModel =new ExchangeModel();
            if (values.Any())
            {

                exchangeModel.Currency = currency;
                exchangeModel.Date = DateTime.UtcNow; // Assign the current date (you can get it from the external API if it's available)

                if (currency.Equals("BRL", StringComparison.OrdinalIgnoreCase))
                {
                    // Perform the conversion if necessary
                    exchangeModel.ExchangeRatePurchase = Convert.ToDecimal(values[0]) / 4;
                    exchangeModel.ExchangeRateSale = Convert.ToDecimal(values[1]) / 4;

                }
                else 
                {
                    exchangeModel.ExchangeRatePurchase = Convert.ToDecimal(values[0]);
                    exchangeModel.ExchangeRateSale = Convert.ToDecimal(values[1]);
                }
            }

  
            return Ok(exchangeModel);

           // return Ok(values);
        }

        //CrossCoree
        // errors in constants
        public void ValidateCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency))
                throw new ApplicationException("currenty type is empty. should be USD or BRL");
            if (!currency.Equals("USD", StringComparison.OrdinalIgnoreCase) &&
                !currency.Equals("BRL", StringComparison.OrdinalIgnoreCase))
                throw new ApplicationException($"{currency} currency type is invalid. should be USD or BRL");
        }

    }
}

