using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WundergroundModels.Alerts
{

    public class Rootobject
    {
        public Response response { get; set; }
        public Alert[] alerts { get; set; }
    }

    public class Response
    {
        public string version { get; set; }
        public string termsofService { get; set; }
        public Features features { get; set; }
    }

    public class Features
    {
        public int alerts { get; set; }
    }

    public class Alert
    {
        public string type { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public string date_epoch { get; set; }
        public string expires { get; set; }
        public string expires_epoch { get; set; }
        public string message { get; set; }
        public string phenomena { get; set; }
        public string significance { get; set; }
        public Zone[] ZONES { get; set; }
        public Stormbased StormBased { get; set; }
    }

    public class Stormbased
    {
        public Vertex[] vertices { get; set; }
        public int Vertex_count { get; set; }
        public Storminfo stormInfo { get; set; }
    }

    public class Storminfo
    {
        public int time_epoch { get; set; }
        public int Motion_deg { get; set; }
        public int Motion_spd { get; set; }
        public float position_lat { get; set; }
        public float position_lon { get; set; }
    }

    public class Vertex
    {
        public string lat { get; set; }
        public string lon { get; set; }
    }

    public class Zone
    {
        public string state { get; set; }
        public string ZONE { get; set; }
    }

}
