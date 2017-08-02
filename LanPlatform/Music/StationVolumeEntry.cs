using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.Music
{
    public class StationVolumeEntry : EditableDatabaseObject
    {
        public long Station { get; set; }
        public long User { get; set; }

        public byte Volume
        {
            get { return SetVolume; }
            set
            {
                if (value > 100)
                    SetVolume = 100;
                else
                    SetVolume = value;
            }
        }

        private byte SetVolume;

        public StationVolumeEntry()
        {
            Station = 0;
            User = 0;

            SetVolume = 100;
        }
    }
}