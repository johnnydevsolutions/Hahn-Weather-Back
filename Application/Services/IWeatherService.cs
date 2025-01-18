using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();
        Task<WeatherForecast> GetForecastByIdAsync(int id);
        Task<WeatherForecast> GetForecastByCityAsync(string city);
        Task UpsertForecastAsync(WeatherForecast forecast);
        Task DeleteForecastAsync(int id);
    }
}