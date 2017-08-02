using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Models;

namespace GabionPlatform.Auth
{
    public class AuthSessionAttempt : AuthAttempt
    {
        public long SessionId { get; set; }
        public String Key { get; set; }

        public AuthSessionAttempt()
        {
            
        }

        public AuthSessionAttempt(AppInstance instance, long id, String key)
            : base(false, instance)
        {
            SessionId = id;
            Key = key;
        }
    }
}