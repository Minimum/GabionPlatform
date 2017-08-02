using System;
using GabionAdmin.Database;

namespace GabionAdmin.Accounts
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