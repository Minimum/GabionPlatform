using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.Music
{
    public class SongUpload : DatabaseObject
    {
        // Upload Details
        public SongUploadType UploadType { get; set; }
        public String UploadInfo { get; set; }

        // Song Details
        public String Title { get; set; }
        public String Artist { get; set; }
        public String Album { get; set; }
        public int TrackNumber { get; set; }
        public long Duration { get; set; }

        public SongUpload()
        {
            UploadType = SongUploadType.File;
            UploadInfo = "";

            Title = "";
            Artist = "";
            Album = "";
            TrackNumber = 0;
            Duration = 0;
        }
    }

    public enum SongUploadType
    {
        File = 0,
        Youtube
    }
}