using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class WeatherForecast : IWeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    }

    public interface IWeatherForecast
    {

    }


    public class WeatherForecastContainer
    {
        public string Json { get; set; }
        public string Type { get; private set; }
        public WeatherForecastContainer(string json, string type)
        {
            Json = json;
            Type = type;
        }
    }
}
