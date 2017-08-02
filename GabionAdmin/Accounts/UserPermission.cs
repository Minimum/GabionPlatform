using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionAdmin.Database;

namespace GabionAdmin.Accounts
{
    public class UserPermission : EditableDatabaseObject
    {
        public long Role { get; set; }
        public String Scope { get; set; }
        public String Flag { get; set; }

        public UserPermission()
        {
            Role = 0;
            Scope = "";
            Flag = "";
        }

        public bool CheckAccess(String flag)
        {
            return CheckFlag("root") || CheckFlag(flag);
        }

        public bool CheckFlag(String flag)
        {
            return Flag.Equals(flag, StringComparison.OrdinalIgnoreCase);
        }
    }
}