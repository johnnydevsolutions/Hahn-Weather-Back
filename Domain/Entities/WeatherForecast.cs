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
        public string Condition { get; set; }
    }
}