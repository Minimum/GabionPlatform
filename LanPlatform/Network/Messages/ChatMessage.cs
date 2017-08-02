using System;

namespace GabionPlatform.Network.Messages
{
    public class ChatMessage : NetMessage
    {
        public long Room { get; set; }
        public long Author { get; set; }
        public String Message { get; set; }

        public override string GetMessageType()
        {
            return "Chat";
        }
    }
}