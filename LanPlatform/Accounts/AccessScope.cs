using GabionPlatform.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabionPlatform.Accounts
{
    public class AccessScope : EditableDatabaseObject
    {
        public String Name { get; set; }
    }
}