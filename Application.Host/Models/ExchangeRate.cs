namespace Application.Host.Models
{
    public class ExchangeModel
    {
        public string Currency { get; set; }  // Ejemplo de propiedad para el tipo de moneda (USD, BRL, etc.).
        public decimal ExchangeRatePurchase { get; set; }  // Ejemplo de propiedad para la tasa de cambio.
        public decimal ExchangeRateSale { get; set; }  // Ejemplo de propiedad para la tasa de cambio.
        public DateTime Date { get; set; }  // Ejemplo de propiedad para la fecha de la tasa de cambio.
    }
}
