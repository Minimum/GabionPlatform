using System;

namespace GabionPlatform.Network.Messages
{
    public class AccountInfoChangeMessage : NetMessage
    {
        public long User { get; set; }
        public String Field { get; set; }
        public String Data { get; set; }

        public override string GetMessageType()
        {
            return "AccountInfoChange";
        }
    }
}