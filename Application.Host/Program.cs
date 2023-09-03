using Application.Core.Repositories;
using Application.Data.Application.Data.Context;
using Application.Data.Context;
using Application.Host.Middleware;
using Application.Host.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Registra ExchangeService como un servicio
//builder.Services.AddScoped<ExchangeService>();
builder.Services.AddScoped<IExchangeService, ExchangeService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<CurrencyExchangeService>();
// Configura Entity Framework Core con CurrencyExchangeContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CurrencyExchangeContext>(options =>
{
    options.UseSqlServer(connectionString,
        options => options.MigrationsAssembly("Application.Data")); // Replace with your actual namespace
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

