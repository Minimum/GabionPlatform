using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.GOnline.Properties
{
    public class Property : EditableDatabaseObject
    {
        public long Region { get; set; }

        public String Name { get; set; }
        public bool ForSale { get; set; }
        public long SalePrice { get; set; }
    }
}