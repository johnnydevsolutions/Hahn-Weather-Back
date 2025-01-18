using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
         private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync()
        {
            return await _context.WeatherForecasts.ToListAsync();
        }

        public async Task<WeatherForecast> GetForecastByIdAsync(int id)
        {
            return await _context.WeatherForecasts.FindAsync(id);
        }

        public async Task AddForecastAsync(WeatherForecast forecast)
        {
            _context.WeatherForecasts.Add(forecast);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForecastAsync(WeatherForecast forecast)
        {
            _context.WeatherForecasts.Update(forecast);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteForecastAsync(int id)
        {
            var forecast = await _context.WeatherForecasts.FindAsync(id);
            if (forecast != null)
            {
                _context.WeatherForecasts.Remove(forecast);
                await _context.SaveChangesAsync();
            }
        }
    }
}