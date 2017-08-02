using GabionAdmin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabionAdmin.Accounts
{
    public class AccessScope : EditableDatabaseObject
    {
        public String Name { get; set; }
    }
}