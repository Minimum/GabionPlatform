using System;
using System.Collections.Generic;
using GabionPlatform.Database;

namespace GabionPlatform.Accounts
{
    public class UserRole : EditableDatabaseObject
    {
        public String Name { get; set; }

        public UserRole()
        {
            Name = "";
        }
    }
}