using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Domain.Exceptions;

namespace Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<int, string> _weatherConditions = new()
        {
            { 0, "Clear sky" },
            { 1, "Mainly clear" },
            { 2, "Partly cloudy" },
            { 3, "Overcast" },
            { 45, "Foggy" },
            { 48, "Depositing rime fog" },
            { 51, "Light drizzle" },
            { 53, "Moderate drizzle" },
            { 55, "Dense drizzle" },
            { 61, "Slight rain" },
            { 63, "Moderate rain" },
            { 65, "Heavy rain" },
            { 71, "Slight snow fall" },
            { 73, "Moderate snow fall" },
            { 75, "Heavy snow fall" },
            { 77, "Snow grains" },
            { 80, "Slight rain showers" },
            { 81, "Moderate rain showers" },
            { 82, "Violent rain showers" },
            { 85, "Slight snow showers" },
            { 86, "Heavy snow showers" },
            { 95, "Thunderstorm" },
            { 96, "Thunderstorm with slight hail" },
            { 99, "Thunderstorm with heavy hail" }
        };

        private readonly ILogger<WeatherService> _logger;
        private readonly IMemoryCache _cache;

        public WeatherService(
            IWeatherRepository weatherRepository, 
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<WeatherService> logger,
            IMemoryCache cache)
        {
            _weatherRepository = weatherRepository;
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync()
        {
            return await _weatherRepository.GetAllForecastsAsync();
        }

        public async Task<WeatherForecast> GetForecastByIdAsync(int id)
        {
            return await _weatherRepository.GetForecastByIdAsync(id);
        }

        public async Task<WeatherForecast> GetForecastByCityAsync(string city)
        {
            var cacheKey = $"weather_{city}";
            if (_cache.TryGetValue(cacheKey, out WeatherForecast cachedForecast))
            {
                return cachedForecast;
            }

            try
            {
                var coordinates = _configuration
                    .GetSection($"WeatherSettings:LatitudeLongitude:{city}")
                    .Get<Dictionary<string, string>>();

                if (coordinates == null)
                    throw new ArgumentException($"Coordinates not found for city: {city}");

                var latitude = coordinates["Latitude"];
                var longitude = coordinates["Longitude"];

                var response = await _httpClient.GetStringAsync(
                    $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true");
                var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

                var weatherCode = (int)weatherData["current_weather"]["weathercode"];
                var condition = _weatherConditions.TryGetValue(weatherCode, out var conditionText) 
                    ? conditionText 
                    : $"Unknown condition ({weatherCode})";

                var temperatureCelsius = (double)weatherData["current_weather"]["temperature"];
                var forecast = new WeatherForecast
                {
                    City = city,
                    Date = DateTime.Now,
                    Temperature = temperatureCelsius,
                    TemperatureFahrenheit = CelsiusToFahrenheit(temperatureCelsius),
                    Condition = condition
                };

                _cache.Set(cacheKey, forecast, TimeSpan.FromMinutes(10));
                return forecast;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                _logger.LogWarning($"Rate limit reached for weather API. City: {city}");
                throw new RateLimitExceededException("Weather API rate limit reached. Please try again later.");
            }
        }

        public async Task UpsertForecastAsync(WeatherForecast forecast)
        {
            var existingForecast = await _weatherRepository.GetForecastByIdAsync(forecast.Id);
            if (existingForecast == null)
            {
                await _weatherRepository.AddForecastAsync(forecast);
            }
            else
            {
                existingForecast.City = forecast.City;
                existingForecast.Date = forecast.Date;
                existingForecast.Temperature = forecast.Temperature;
                existingForecast.Condition = forecast.Condition;
                await _weatherRepository.UpdateForecastAsync(existingForecast);
            }
        }

        public async Task DeleteForecastAsync(int id)
        {
            await _weatherRepository.DeleteForecastAsync(id);
        }

        private double CelsiusToFahrenheit(double celsius) => (celsius * 9/5) + 32;
    }
}