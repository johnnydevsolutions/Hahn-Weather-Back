using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public class WeatherForecastDto
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double TemperatureFahrenheit { get; set; }
        public string? Condition { get; set; }
    }
}