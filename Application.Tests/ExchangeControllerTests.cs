using Application.Host.Controllers;
using Application.Host.Models;
using Application.Host.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class ExchangeControllerTests
    {
        private readonly ExchangeController _controller;
        private readonly Mock<ILogger<ExchangeController>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;

        public ExchangeControllerTests()
        {
            // Initialize mock dependencies
            _loggerMock = new Mock<ILogger<ExchangeController>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            // Configure the behavior of the IHttpClientFactory mock as needed
            // For example, setting up the CreateClient method to return a mock HttpClient

            // Create an instance of the ExchangeService with the IHttpClientFactory mock
            var exchangeService = new ExchangeService(_httpClientFactoryMock.Object);

            // Create an instance of the controller with mock dependencies
            _controller = new ExchangeController(
                _loggerMock.Object,
                exchangeService
            );
        }

        [Fact]
        public async Task GetByCurrency_ReturnsOkResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExchangeController>>();
            var exchangeServiceMock = new Mock<IExchangeService>();


            var expectedCurrency = "USD";
            var expectedValues = new string[] { "4.5", "4.6" }; // Example exchange rates

            exchangeServiceMock
                .Setup(service => service.GetValuesFromExternalAPI())
                .ReturnsAsync(expectedValues);

            var controller = new ExchangeController(loggerMock.Object, exchangeServiceMock.Object);

            // Act
            var result = await controller.GetByCurrency(expectedCurrency);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var exchangeModel = Assert.IsType<ExchangeModel>(okResult.Value);

            Assert.Equal(expectedCurrency, exchangeModel.Currency);
            Assert.True(exchangeModel.Date <= DateTime.UtcNow);
            Assert.Equal(decimal.Parse(expectedValues[0]), exchangeModel.ExchangeRatePurchase);
            Assert.Equal(decimal.Parse(expectedValues[1]), exchangeModel.ExchangeRateSale);
        }

   

        [Fact]
        public async Task ValidateCurrency_InvalidCurrency_ThrowsException2()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExchangeController>>();
            var exchangeServiceMock = new Mock<ExchangeService>();

            //var controller = new ExchangeController(loggerMock.Object, exchangeServiceMock.Object);

            // Act and Assert
            // var exception =  Assert.ThrowsAsync<ApplicationException>(() => _controller.GetByCurrency("EUR"));
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _controller.GetByCurrency("EUR"));

            // Assert: Verifica el mensaje de error en la excepción
            // Assert.Equal("EUR currency type is invalid. should be USD or BRL", exception);
            Assert.Equal("EUR currency type is invalid. should be USD or BRL", exception.Message);

        }


    }
}
