using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabionPlatform.Models.Requests
{
    public class SetSteamCodeRequest
    {
        public String Username { get; set; }
        public String Code { get; set; }
        public long Challenge { get; set; }

        public SetSteamCodeRequest()
        {
            Username = "";
            Code = "";
            Challenge = -1;
        }
    }
}