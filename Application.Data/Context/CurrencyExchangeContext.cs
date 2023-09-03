using Application.Data.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Context
{
    public class CurrencyExchangeContext : DbContext
    {
        public CurrencyExchangeContext(DbContextOptions<CurrencyExchangeContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             // Entity Framework Core will automatically generate the model configurations
             // based on our entity classes.
        }

    }
}
