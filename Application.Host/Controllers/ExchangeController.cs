using Microsoft.AspNetCore.Mvc;

namespace Application.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeController : Controller
    {

        //1- Domain, CrossCore

        [HttpPost(Name = "get-by-currenry")]
        public async Task<IActionResult> GetByCurrency(string currency)
        {
            ValidateCurrency(currency);
            var values = await GetValues();
            if (values.Any())
            {
                if (currency.Equals("BRL", StringComparison.OrdinalIgnoreCase))
                {
                    values[0] = Convert.ToString(Convert.ToDouble(values[0]) / 4);
                    values[1] = Convert.ToString(Convert.ToDouble(values[0]) / 4);
                }
            }
            return Ok(values);
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
        private static async Task<string[]> GetValues()
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
        }
    }
}

