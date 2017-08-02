using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GabionCache.Settings
{
    public class ProxySettings
    {
        public List<IPAddress> DnsAddresses { get; set; }
        public List<String> ListenAddresses { get; set; } 
        public int ListenPort { get; set; }
        public int BufferSize { get; set; }
    }
}
