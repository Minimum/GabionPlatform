using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.Awards
{
    public class ExperienceEntry : DatabaseObject
    {
        public long Account { get; set; }
        public int Amount { get; set; }
        public long Time { get; set; }

        public ExperienceEntry()
        {
            Account = 0;
            Amount = 0;
            Time = 0;
        }
    }
}