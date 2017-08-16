using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.Apps
{
    public class AppEditRecord : DatabaseObject
    {
        public long Editor { get; set; }
        public long Time { get; set; }
        public String Field { get; set; }
        public String NewValue { get; set; }
        public String OldValue { get; set; }

        public AppEditRecord()
        {
            Editor = 0;
            Time = 0;
            Field = "";
            NewValue = "";
            OldValue = "";
        }
    }
}