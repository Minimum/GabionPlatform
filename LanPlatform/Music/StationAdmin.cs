using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.Music
{
    public class StationAdmin : DatabaseObject
    {
        public long Station { get; set; }
        public long User { get; set; }

        public StationAdmin()
        {
            Station = 0;
            User = 0;
        }
    }
}