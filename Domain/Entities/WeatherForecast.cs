using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double TemperatureFahrenheit { get; set; }
        public string Condition { get; set; }
        public double WindSpeed { get; set; }
        public double Humidity { get; set; }
        public double FeelsLike { get; set; }
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
        public double TemperatureAverage { get; set; }
    }
}