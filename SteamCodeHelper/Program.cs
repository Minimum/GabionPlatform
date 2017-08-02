using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;

namespace SteamCodeHelper
{
    public class Program
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        private static string ApplicationName = "SteamCodeGrabber";

        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Usage: SteamCodeHelper <SteamAccount> <PlatformChallenge>");

                return;
            }

            UserCredential credential;
            PlatformCredentials platform;

            using (var stream = new FileStream("platform.json", FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                platform = JsonConvert.DeserializeObject<PlatformCredentials>(reader.ReadToEnd());
            }

            using (var stream =
                new FileStream("googleClientSecret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.CurrentDirectory;
                credPath = Path.Combine(credPath, "googleCredentials");


                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            GmailService gmail = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            SteamCodeGrabber grabber = new SteamCodeGrabber(platform, gmail, credential, args[0], args[1]);

            grabber.Start();

            bool run = true;

            while (run)
            {
                String command = Console.ReadLine();

                run = command != null && command.Equals("exit") && grabber.Running;
            }

            if(grabber.Running)
                grabber.Stop();

            return;
        }
    }
}
