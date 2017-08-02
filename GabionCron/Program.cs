using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GabionCron
{
    public class Program
    {
        private const String ApiAddress = "https://gslans.net/api/";

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                using (WebClient client = new WebClient())
                {
                    System.Console.WriteLine("Contacting WebService at \""+ApiAddress+args[0]+"\"...");
                    
                    try
                    {
                        String result = "";

                        result = client.DownloadString(ApiAddress + args[0]);

                        System.Console.WriteLine("Successfully contacted WebService.\nReturn Info:\n"+result);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Error contacting WebService.\nError Info:\n"+e.Message);
                    }
                }
            }
            else
            {
                System.Console.WriteLine("Usage: gabioncron <ApiPath>");
            }
        }
    }
}
