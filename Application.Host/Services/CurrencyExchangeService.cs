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
        // Validate monthly limit
        var monthlyLimit = GetMonthlyLimit(request.CurrencyCode);
        var totalPurchasesThisMonth = _transactionRepository.GetTotalPurchasesThisMonth(request.UserId, request.CurrencyCode);

        if (totalPurchasesThisMonth == 0)
        {
            return new ValidationResult(false, "Verify that you have entered the information correctly.");
        }

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

    public IEnumerable<Transaction> GetTransactionsByUserId(string userId)
    {
        try
        {
            // Call the repository to retrieve transactions for the specified user
            var transactions = _transactionRepository.GetTransactionsByUserId(userId);

            return transactions;
        }
        catch (Exception ex)
        {
            // Log the error
            // Implement proper error handling/logging here
            throw; // Rethrow the exception or handle it as needed
        }
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
        // Implement logic to get the exchange rate based on the currency code
        // This could involve making an external API call or querying a database
        // For example, you can simulate a fixed exchange rate for demo purposes:
        switch (currencyCode)
        {
            case "USD":
                return 1.0m; // Fixed exchange rate for USD (1 USD = 1 USD)
            case "BRL":
                return 5.0m; // Fixed exchange rate for BRL (1 USD = 5 BRL)
            default:
                return 0.0m; // Invalid currency code
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
