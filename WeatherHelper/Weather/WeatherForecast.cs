using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherHelper.Weather
{
    public class WeatherReport
    {
        public long Time { get; set; }

        // General
        public WeatherCondition Condition { get; set; } // Weather condition
        public int Temperature { get; set; }            // Temperature in F
        public int FeelsLike { get; set; }              // Feels like temperature in F
        public int Humidity { get; set; }               // Humidity in percent
        public int DewPoint { get; set; }               // Dew point in F
        public int AirPressure { get; set; }          // Air pressure in metric
        public int UVRating { get; set; }               // UV Rating

        // Wind
        public int WindSpeed { get; set; }              // Wind speed in MPH
        public int WindDirection { get; set; }          // Wind direction in degrees

        public WeatherReport()
        {
            Condition = WeatherCondition.Unknown;
            Temperature = 0;
            FeelsLike = 0;
            Humidity = 0;
            DewPoint = 0;
            AirPressure = 0;
            UVRating = 0;
        }
    }

    public enum WeatherCondition
    {
        Unknown = 0,
        Clear,
        Overcast,
        ScatteredClouds,
        PartlyCloudy,
        MostlyCloudy,
        Fog,
        Drizzle,
        Rain,
        Thunderstorm,
        Hail,
        Snow,
        Smoke,
        VolcanicAsh,
        Sandstorm
    }
}
