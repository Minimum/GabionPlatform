using GabionPlatform.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabionPlatform.Models
{
    public class AuthUsernameModel
    {
        public long Id { get; set; }
        public long Account { get; set; }
        public String Username { get; set; }

        public AuthUsernameModel(AuthUsername auth)
        {
            Id = auth.Id;
            Account = auth.Account;
            Username = auth.Username;
        }
    }
}