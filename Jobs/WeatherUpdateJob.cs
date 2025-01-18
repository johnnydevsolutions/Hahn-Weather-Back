using Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Jobs
{
    public class WeatherUpdateJob
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherUpdateJob> _logger;

        public WeatherUpdateJob(IWeatherService weatherService, ILogger<WeatherUpdateJob> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        public async Task ExecuteAsync(string city)
        {
            try
            {
                // Obter os dados do serviço de clima
                var forecast = await _weatherService.GetForecastByCityAsync(city);

                // Realizar o upsert
                await _weatherService.UpsertForecastAsync(forecast);

                _logger.LogInformation($"Weather data upserted for city: {city} at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while upserting weather data for city: {city}");
            }
        }
    }
}