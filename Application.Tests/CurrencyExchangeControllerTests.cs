using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Repositories;
using Application.Data.Context;
using Application.Data.Models;
using Application.Host.Controllers;
using Application.Host.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Application.Tests
{
    public class CurrencyExchangeControllerTests
    {
        private readonly CurrencyExchangeController _controller;

        public CurrencyExchangeControllerTests()
        {
            _controller = BuildController();
        }

        private CurrencyExchangeController BuildController()
        {
            /*var options = new DbContextOptionsBuilder<CurrencyExchangeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;*/

            var serviceProvider = new ServiceCollection()
                .AddDbContext<CurrencyExchangeContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                })
                .AddScoped<UserRepository>()
                .AddScoped<TransactionRepository>()
                .AddScoped<CurrencyExchangeService>()
                .BuildServiceProvider();

            var currencyExchangeService = serviceProvider.GetService<CurrencyExchangeService>();

            return new CurrencyExchangeController(currencyExchangeService);
        }

        [Fact]
        public async Task PurchaseCurrency_ValidRequest_ReturnsOk()
        {
            // Arrange: Configura una solicitud HTTP POST con un cuerpo JSON
            var request = new CurrencyPurchaseRequest
            {
                UserId = "user123",
                Amount = 100.0m,
                CurrencyCode = "USD"
            };

            // Act: Ejecuta el método PurchaseCurrency del controlador
            var result = _controller.PurchaseCurrency(request);

            // Assert: Verifica que el resultado sea un OkObjectResult y contiene el valor esperado
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(100.0m, returnValue);

            // Puedes agregar más verificaciones según tus requisitos.
        }

        [Fact]
        public async Task GetTransactions_ValidUserId_ReturnsOk()
        {
            // Arrange: Configura una solicitud HTTP GET
            var userId = "user123";

            // Act: Ejecuta el método GetTransactions del controlador
            var result = _controller.GetTransactions(userId);

            // Assert: Verifica que el resultado sea un OkObjectResult y contiene los datos esperados
            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactions = Assert.IsAssignableFrom<IEnumerable<Transaction>>(okResult.Value);

            // Puedes agregar más verificaciones según tus requisitos.
        }
    }
}
