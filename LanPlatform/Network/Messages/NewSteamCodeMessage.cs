using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabionPlatform.Network.Messages
{
    public class NewSteamCodeMessage : NetMessage
    {
        public String Code { get; set; }

        public override string GetMessageType()
        {
            return "NewSteamCode";
        }

        public NewSteamCodeMessage()
        {
            Code = "";
        }

        public NewSteamCodeMessage(String code)
        {
            Code = code;
        }
    }
}