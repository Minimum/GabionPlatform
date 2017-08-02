﻿using System;
using System.Collections.Generic;
using GabionPlatform.Database;

namespace GabionPlatform.Events
{
    public class LanEvent : EditableDatabaseObject
    {
        public String Name { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }

        public List<LanEventGuest> GuestRecords { get; set; }

        public LanEvent()
        {
            Name = "";
            StartTime = 0;
            EndTime = 0;

            GuestRecords = new List<LanEventGuest>();
        }
    }
}