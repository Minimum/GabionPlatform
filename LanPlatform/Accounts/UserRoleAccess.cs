using System;
using GabionPlatform.Database;

namespace GabionPlatform.Accounts
{
    public class UserRoleAccess : EditableDatabaseObject
    {
        public long Role { get; set; }
        public long User { get; set; }

        public UserRoleAccess()
        {
            Role = 0;
            User = 0;
        }
    }
}