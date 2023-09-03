namespace Application.Host.Services
{
    public class ExchangeService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExchangeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

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

        public async Task<string[]> GetValuesFromExternalAPI()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://www.bancoprovincia.com.ar/Principal/dolar");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        // Supongamos que la respuesta es un array JSON de cadenas
                        return System.Text.Json.JsonSerializer.Deserialize<string[]>(content);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error de solicitud aquí
                // Puedes registrar errores o lanzar excepciones personalizadas según tus necesidades
            }

            return null;
        }
    }
}
