using Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Jobs
{
    public class WeatherUpdateJob
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherUpdateJob> _logger;
        private readonly IConfiguration _configuration;

        public WeatherUpdateJob(
            IWeatherService weatherService, 
            ILogger<WeatherUpdateJob> logger,
            IConfiguration configuration)
        {
            _weatherService = weatherService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ExecuteAsync()
        {
            var cities = _configuration.GetSection("WeatherSettings:Cities").Get<List<string>>();
            
            foreach (var city in cities)
            {
                try
                {
                    var forecast = await _weatherService.GetForecastByCityAsync(city);
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
}