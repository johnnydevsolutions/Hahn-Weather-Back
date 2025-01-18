using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetStringAsync($"https://api.open-meteo.com/v1/forecast?latitude=35.6895&longitude=139.6917&current_weather=true");
            var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

            return new WeatherForecast
            {
                City = city,
                Date = DateTime.Now,
                Temperature = weatherData["current_weather"]["temperature"],
                Condition = weatherData["current_weather"]["weathercode"].ToString()
            };
        }
    }
}