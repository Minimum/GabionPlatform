using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Thread = System.Threading.Thread;

namespace SteamCodeHelper
{
    public class SteamCodeGrabber
    {
        // Platform Info
        public PlatformCredentials PlatformInfo { get; set; }

        // Google Info
        public GmailService MailService { get; set; }
        public UserCredential GoogleCredential { get; set; }

        // Account Info
        public String TargetAccount { get; set; }
        public String Challenge { get; set; }

        private readonly BackgroundWorker Worker;

        public bool Running
        {
            get { return Worker.IsBusy; }
        }

        public SteamCodeGrabber(PlatformCredentials platform, GmailService mail, UserCredential google, String targetAccount, String challenge)
        {
            PlatformInfo = platform;
            MailService = mail;
            GoogleCredential = google;

            TargetAccount = targetAccount;
            Challenge = challenge;

            Worker = new BackgroundWorker();

            Worker.WorkerSupportsCancellation = true;

            Worker.DoWork += WorkerThread;
        }

        public void Start()
        {
            if (!Worker.IsBusy)
            {
                Worker.RunWorkerAsync();
            }

            return;
        }

        public void Stop()
        {
            Worker.CancelAsync();

            return;
        }

        private void WorkerThread(object sender, DoWorkEventArgs e)
        {
            String lastId = "";
            bool found = false;
            int runs = 0;

            while (!Worker.CancellationPending && !found && runs < 50)
            {
                List<Message> messages = ListMessages(MailService, "me", "Access from new computer");

                bool firstMessage = true;

                foreach (Message messageRef in messages)
                {
                    if (messageRef.Id.Equals(lastId, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    else
                    {
                        if (firstMessage)
                        {
                            lastId = messageRef.Id;

                            firstMessage = false;
                        }

                        Thread.Sleep(1050);

                        Message message = GetMessage(MailService, "me", messageRef.Id);

                        if (message.Raw != null)
                        {
                            String encodedMessage = message.Raw.Replace('_', '/').Replace('-', '+');

                            switch (message.Raw.Length % 4)
                            {
                                case 2: encodedMessage += "=="; break;
                                case 3: encodedMessage += "="; break;
                            }

                            byte[] data = Convert.FromBase64String(encodedMessage);
                            
                            String decodedString = Encoding.UTF8.GetString(data);

                            int accountStartLocation =
                                decodedString.IndexOf("Here is the Steam Guard code you need to login to account", StringComparison.Ordinal) + 58;

                            if (accountStartLocation != 57)
                            {
                                int accountEndLocation = decodedString.IndexOf(":\r\n", accountStartLocation, StringComparison.OrdinalIgnoreCase);

                                String accountName = decodedString.Substring(accountStartLocation,
                                    accountEndLocation - accountStartLocation);
                                
                                int codeStartLocation = accountEndLocation + 3;

                                int codeEndLocation = decodedString.IndexOf("\r\n", codeStartLocation, StringComparison.OrdinalIgnoreCase);

                                String accountCode = decodedString.Substring(codeStartLocation, 
                                    codeEndLocation - codeStartLocation);

                                System.Console.WriteLine("SteamGuard Email\nAccount: " + accountName + "\nCode: " + accountCode + "\n");

                                if (accountName.Equals(TargetAccount, StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        SendPlatformDetails(accountName, accountCode);

                                        ModifyMessageRequest mods = new ModifyMessageRequest();
                                        mods.RemoveLabelIds = new List<String>();
                                        mods.RemoveLabelIds.Add("UNREAD");

                                        Thread.Sleep(1050);

                                        MailService.Users.Messages.Modify(mods, "me", message.Id).Execute();

                                        found = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("An error occurred: " + ex.Message);
                                    }
                                }
                            }
                        }
                    }

                    runs++;
                }

                // Sleep for 3 seconds
                Thread.Sleep(3000);   
            }

            return;
        }

        private void SendPlatformDetails(String username, String code)
        {
            Uri baseAddress = new Uri(PlatformInfo.Url);
            CookieContainer cookies = new CookieContainer();

            using(HttpClientHandler handler = new HttpClientHandler() {CookieContainer = cookies})
            using (HttpClient client = new HttpClient(handler))
            {
                cookies.Add(baseAddress, new Cookie("LPSessionId", PlatformInfo.UserId));
                cookies.Add(baseAddress, new Cookie("LPSessionKey", PlatformInfo.SessionKey));

                Dictionary<String, String> values = new Dictionary<String, String>
                {
                   { "Username", username },
                   { "Code", code },
                   { "Challenge", Challenge }
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                HttpResponseMessage result = client.PostAsync(PlatformInfo.Url + "/api/loaners/steamcode", content).Result;
                result.EnsureSuccessStatusCode();
            }

            return;
        }

        private static List<Message> ListMessages(GmailService service, String userId, String query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            request.Q = query;
            request.MaxResults = 10;

            do
            {
                try
                {
                    ListMessagesResponse response = request.Execute();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

        private static Message GetMessage(GmailService service, String userId, String messageId)
        {
            try
            {
                UsersResource.MessagesResource.GetRequest request = service.Users.Messages.Get(userId, messageId);

                request.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Raw;

                return request.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return null;
        }


    }
}
