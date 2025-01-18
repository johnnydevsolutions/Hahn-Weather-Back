using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Newtonsoft.Json;

namespace Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly HttpClient _httpClient;

        public WeatherService(IWeatherRepository weatherRepository, HttpClient httpClient)
        {
            _weatherRepository = weatherRepository;
            _httpClient = httpClient;
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
            // Mapeie a cidade para latitude e longitude conforme necessário
            // Exemplo para São Paulo: latitude=-23.5505, longitude=-46.6333
            var latitude = "-23.5505";
            var longitude = "-46.6333";

            var response = await _httpClient.GetStringAsync($"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true");
            var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

            return new WeatherForecast
            {
                City = city,
                Date = DateTime.Now,
                Temperature = (double)weatherData["current_weather"]["temperature"],
                Condition = weatherData["current_weather"]["weathercode"].ToString()
            };
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
    }
}