using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GabionPlatform.Database;
using Newtonsoft.Json;

namespace GabionPlatform.Apps
{
    public class LoanerAccount : EditableDatabaseObject
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String SteamCode { get; set; }
        public long CheckoutUser { get; set; }

        [JsonIgnore]
        public long CheckoutChallenge { get; set; }

        [NotMapped]
        public List<LoanerApp> Apps { get; set; } 

        public LoanerAccount()
        {
            Username = "";
            Password = "";
            SteamCode = "";
            CheckoutUser = 0;
            CheckoutChallenge = 1;

            Apps = new List<LoanerApp>();
        }
    }
}