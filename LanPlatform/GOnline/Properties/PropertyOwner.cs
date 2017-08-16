using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.GOnline.Properties
{
    public class PropertyOwner : EditableDatabaseObject
    {
        public long Owner { get; set; }
        public long Property { get; set; }
        public int Equity { get; set; }
    }
}