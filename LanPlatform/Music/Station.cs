using System;
using GabionPlatform.Database;

namespace GabionPlatform.Music
{
    public class Station : EditableDatabaseObject
    {
        public String Name { get; set; }

        public int Volume { get; set; }
        public bool VolumeOverride { get; set; }

        // admins, etc

        public Station()
        {
            Name = "";

            Volume = 100;
            VolumeOverride = false;
        }
    }
}