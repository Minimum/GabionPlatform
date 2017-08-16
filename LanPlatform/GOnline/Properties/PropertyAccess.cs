using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.GOnline.Properties
{
    public class PropertyAccess : EditableDatabaseObject
    {
        public long User { get; set; }
        public long Property { get; set; }
        public String AccessCode { get; set; }
    }
}