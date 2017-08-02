using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherHelper
{
    public class WeatherGrabber
    {
        public String RemoteApiLocation { get; set; }
        public String RemoteApiKey { get; set; }

        public String LocalApiLocation { get; set; }
        public String LocalApiSessionId { get; set; }
        public String LocalApiSessionKey { get; set; }

        [JsonIgnore]
        public String LocationState { get; set; }
        [JsonIgnore]
        public String LocationCity { get; set; }

        public WeatherGrabber()
        {
            RemoteApiKey = "";
            RemoteApiLocation = "";

            LocalApiLocation = "";
            LocalApiSessionId = "";
            LocalApiSessionKey = "";

            LocationState = "";
            LocationCity = "";
        }

        public async void GetHourlyInfo()
        {
            // Get info from Wunderground API
            HttpClient client = new HttpClient();

            String output = await client.GetStringAsync(RemoteApiLocation + RemoteApiKey + "/hourly/q/" + LocationCity + "/" + LocationState + ".json");

            WundergroundModels.HourlyForecast.Rootobject data =
                JsonConvert.DeserializeObject<WundergroundModels.HourlyForecast.Rootobject>(output);

            // Parse info


            data.hourly_forecast[0].temp.english;

            // Send info to GabionPlatform API
        }

        public void GetHourlyRadar()
        {
            
        }

        public void GetThreeDayInfo()
        {
            
        }

        public void GetAlerts()
        {
            
        }

        public void GetCurrent()
        {
            
        }
    }
}
