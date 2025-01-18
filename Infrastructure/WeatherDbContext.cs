using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    // public class WeatherDbContext : DbContext
    // {
    //     public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

    //     public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    // }

    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        // Configurações adicionais, se necessário
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais de entidades podem ser feitas aqui
        }
    }
}