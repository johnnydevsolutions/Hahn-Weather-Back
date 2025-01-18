using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    public class WeatherDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
    {
        public WeatherDbContext CreateDbContext(string[] args)
        {
            // Aqui você configura manualmente a connection string
            // (exatamente a mesma usada no seu Program.cs, mas hardcoded para design-time)

            var connectionString = "Server=DESKTOP-6B4AKRF\\SQLEXPRESS;Database=WeatherDb;Trusted_Connection=True;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WeatherDbContext(optionsBuilder.Options);
        }
    }
}