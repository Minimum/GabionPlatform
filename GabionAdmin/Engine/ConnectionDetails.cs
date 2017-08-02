using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionAdmin.Engine
{
    public class ConnectionDetails
    {
        public String Address { get; set; }
        public String SessionId { get; set; }
        public String SessionKey { get; set; }

        public ConnectionDetails()
        {
            Address = "https://gslans.net/";

            SessionId = "";
            SessionKey = "";
        }
    }
}
