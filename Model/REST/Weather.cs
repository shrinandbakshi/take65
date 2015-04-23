using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class Weather
    {
        public DateTime dateWeather { get; set; }
        public string dateWeatherLabel { get; set; }
        public string dateLabel { get; set; }
        public long time { get; set; }
        public string icon { get; set; }
        public string summary { get; set; }
        
        public double tempMax { get; set; }
        public double tempMin { get; set; }
        public double currentTemp { get; set; }

        public DateTime lastcache { get; set; }

        public long? sunriseTime { get; set; }
        public long? sunsetTime { get; set; }
        public long? tempMinTime { get; set; }
        public long? tempMaxTime { get; set; }
        public string precipType { get; set; }
        public double? precipProbability { get; set; }
        public double? moonPhase { get; set; }
        public double? dewPoint { get; set; }
        public double? humidity { get; set; }
        public double? windSpeed { get; set; }
        public double? pressure { get; set; }
        public double? visibility { get; set; }
        public double? cloudCover { get; set; }
        public double? ozone { get; set; }       
    }
}
