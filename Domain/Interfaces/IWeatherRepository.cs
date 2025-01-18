using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWeatherRepository
    {
        Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();
        Task<WeatherForecast> GetForecastByIdAsync(int id);
        Task AddForecastAsync(WeatherForecast forecast);
        Task UpdateForecastAsync(WeatherForecast forecast);
        Task DeleteForecastAsync(int id);
    }
}