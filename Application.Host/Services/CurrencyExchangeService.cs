using Application.Host.Controllers;
using System;
//using System.Transactions;
using Application.Data.Models;
using Application.Core.Repositories;

public class CurrencyExchangeService
{
    private readonly UserRepository _userRepository;
    private readonly TransactionRepository _transactionRepository;

    public CurrencyExchangeService(UserRepository userRepository, TransactionRepository transactionRepository)
    {
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
    }

    public ValidationResult ValidatePurchaseRequest(CurrencyPurchaseRequest request)
    {
        // Validate user existence (you can extend this validation as needed)
        var user = _userRepository.GetUserById(request.UserId);
        if (user == null)
        {
            return new ValidationResult(false, "User not found.");
        }

        // Validate monthly limit
        var monthlyLimit = GetMonthlyLimit(request.CurrencyCode);
        var totalPurchasesThisMonth = _transactionRepository.GetTotalPurchasesThisMonth(request.UserId, request.CurrencyCode);

        if (totalPurchasesThisMonth + request.Amount > monthlyLimit)
        {
            return new ValidationResult(false, "Purchase exceeds monthly limit.");
        }

        return new ValidationResult(true);
    }

    public decimal PurchaseCurrency(CurrencyPurchaseRequest request)
    {
        // Perform the currency purchase and store the transaction in the database
        var transaction = new Transaction
        {
            UserId = request.UserId,
            Amount = request.Amount,
            CurrencyCode = request.CurrencyCode,
            Timestamp = DateTime.UtcNow
        };

        _transactionRepository.AddTransaction(transaction);

        // Calculate the resulting value (you need to fetch exchange rates from a source)
        var exchangeRate = GetExchangeRate(request.CurrencyCode);
        var resultingValue = request.Amount / exchangeRate;

        return resultingValue;
    }

    private decimal GetMonthlyLimit(string currencyCode)
    {
        // Implementa lógica para obtener el límite mensual basado en el código de moneda
        // Por ejemplo, establecer límites para "USD" y "BRL"
        switch (currencyCode)
        {
            case "USD":
                return 200.0m; // Límite mensual para USD
            case "BRL":
                return 300.0m; // Límite mensual para BRL
            default:
                return 0.0m;   // Otros códigos de moneda no tienen límite configurado
        }
    }

    private decimal GetExchangeRate(string currencyCode)
    {
        // Implementa lógica para obtener la tasa de cambio basada en el código de moneda
        // Esto podría implicar hacer una llamada a una API externa o consultar una base de datos
        // Por ejemplo, puedes simular una tasa de cambio fija para fines de demostración:
        switch (currencyCode)
        {
            case "USD":
                return 1.0m; // Tasa de cambio fija para USD (1 USD = 1 USD)
            case "BRL":
                return 5.0m; // Tasa de cambio fija para BRL (1 USD = 5 BRL)
            default:
                return 0.0m; // Código de moneda no válido
        }
    }
}

public class ValidationResult
{
    public bool IsValid { get; }
    public string ErrorMessage { get; }

    public ValidationResult(bool isValid, string errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
}
