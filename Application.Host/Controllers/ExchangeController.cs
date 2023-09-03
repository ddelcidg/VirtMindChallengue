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
        private readonly ILogger<ExchangeController> _logger;
        private readonly ExchangeService _exchangeService;
        //1- Domain, CrossCore
        public ExchangeController(ILogger<ExchangeController> logger, ExchangeService exchangeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
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
                /*if (currency.Equals("BRL", StringComparison.OrdinalIgnoreCase))
                {
                    values[0] = Convert.ToString(Convert.ToDouble(values[0]) / 4);
                    values[1] = Convert.ToString(Convert.ToDouble(values[0]) / 4);
                }*/
                exchangeModel.Currency = currency;
                exchangeModel.Date = DateTime.UtcNow; // Asigna la fecha actual (puedes obtenerla de la API externa si está disponible)

                if (currency.Equals("BRL", StringComparison.OrdinalIgnoreCase))
                {
                    // Realiza la conversión si es necesario
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
        //errores en una constantes
        private void ValidateCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency))
                throw new ApplicationException("currenty type is empty. should be USD or BRL");
            if (!currency.Equals("USD", StringComparison.OrdinalIgnoreCase) &&
                !currency.Equals("BRL", StringComparison.OrdinalIgnoreCase))
                throw new ApplicationException($"{currency} currenty type is invalid. should be USD or BRL");
        }

        //Domain
       /* private static async Task<string[]> GetValues()
        {
            string[] response = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage httpResponseMessage = await client.GetAsync("https://www.bancoprovincia.com.ar/Principal/dolar");
            if (httpResponseMessage.IsSuccessStatusCode)
            {                
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                if (content != null)
                    response = System.Text.Json.JsonSerializer.Deserialize<string[]>(content);        
            }
            return response;
        }*/
    }
}

