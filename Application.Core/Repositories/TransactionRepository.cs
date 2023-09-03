﻿using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data.Context;
using Microsoft.EntityFrameworkCore;
using Application.Data.Models;

namespace Application.Core.Repositories
{
    public class TransactionRepository
    {
        private readonly CurrencyExchangeContext _context;

        public TransactionRepository(CurrencyExchangeContext context)
        {
            _context = context;
        }

        // Get transactions for a user
        public List<Transaction> GetTransactionsForUser(string userId)
        {
            return _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Timestamp)
                .ToList();
        }

        // Get the total purchases in a specific currency for the current month
        public decimal GetTotalPurchasesThisMonth(string userId, string currencyCode)
        {
            var currentDate = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            return _context.Transactions
                .Where(t => t.UserId == userId && t.CurrencyCode == currencyCode && t.Timestamp >= firstDayOfMonth)
                .Sum(t => t.Amount);
        }

        // Add a new transaction
        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        // You can add more transaction-related database interactions as needed
    }
}
