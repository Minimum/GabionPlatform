using System;
using GabionPlatform.Database;

namespace GabionPlatform.Apps
{
    public class App : EditableDatabaseObject
    {
        public AppType Type { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public AppDownloadType DownloadType { get; set; }
        public String DownloadInfo { get; set; }

        public App()
        {
            Type = AppType.None;
            Title = "";
            Description = "";
            DownloadType = AppDownloadType.None;
            DownloadInfo = "";
        }
    }

    public enum AppType
    {
        None = 0,
        App,
        Game,
        Mod
    }

    public enum AppDownloadType
    {
        None = 0,
        Url,
        Steam,
        Content
    }
}