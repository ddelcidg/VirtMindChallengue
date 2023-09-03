using Application.Data.Context;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Application.Data
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    namespace Application.Data.Context
    {
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CurrencyExchangeContext>
        {
            public CurrencyExchangeContext CreateDbContext(string[] args)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var config = builder.Build();
                var connectionString = config.GetConnectionString("DefaultConnection");

                var optionsBuilder = new DbContextOptionsBuilder<CurrencyExchangeContext>();
                optionsBuilder.UseSqlServer(connectionString);

                return new CurrencyExchangeContext(optionsBuilder.Options);
            }
        }
    }




}
