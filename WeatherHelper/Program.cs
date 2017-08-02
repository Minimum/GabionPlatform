using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            WeatherGrabber grabber = null;

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: weatherhelper <mode>\nModes:\n0 - Current Weather\n1 - Current Alerts\n2 - Hourly Info\n3 - Hourly Radar\n4 - 3 Day Forecast");

                return;
            }

            try
            {
                using (FileStream stream = new FileStream("settings.json", FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    grabber = JsonConvert.DeserializeObject<WeatherGrabber>(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read settings file.\nError:\n" + e);

                return;
            }

            if (args[0][0] == '0')
            {
                grabber.GetCurrent();
            }
            else if (args[0][0] == '1')
            {
                grabber.GetAlerts();
            }
            else if (args[0][0] == '2')
            {
                grabber.GetHourlyInfo();
            }
            else if (args[0][0] == '3')
            {
                grabber.GetHourlyRadar();
            }
            else if (args[0][0] == '4')
            {
                grabber.GetThreeDayInfo();
            }

        }
    }
}
