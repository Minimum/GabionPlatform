﻿using System;
using System.Collections.Generic;
using GabionPlatform.Database;

namespace GabionPlatform.Awards
{
    public class AwardGroup : EditableDatabaseObject
    {
        public String Name { get; set; }
        public List<Award> Awards { get; set; } 

        public AwardGroup()
        {
            Name = "";
            Awards = new List<Award>();
        }
    }
}