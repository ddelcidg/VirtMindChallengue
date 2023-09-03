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
            // Define entity configurations here if needed
            /*modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Transaction>().HasKey(t => t.TransactionId);*/

            // Configure relationships if applicable
            // Example:
            // modelBuilder.Entity<User>()
            //     .HasMany(u => u.Transactions)
            //     .WithOne(t => t.User)
            //     .HasForeignKey(t => t.UserId);

            // Add more entity configurations and relationships as required

           // base.OnModelCreating(modelBuilder);
        }
    }
}
