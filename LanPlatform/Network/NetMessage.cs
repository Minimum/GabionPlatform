using System;
using System.Collections.Generic;
using GabionPlatform.Database;
using Newtonsoft.Json;

namespace GabionPlatform.Network
{
    public abstract class NetMessage : DatabaseObject
    {
        [JsonIgnore]
        public List<long> Targets { get; set; }

        public NetMessage()
        {
            Targets = new List<long>();
        }

        public abstract String GetMessageType();
    }
}