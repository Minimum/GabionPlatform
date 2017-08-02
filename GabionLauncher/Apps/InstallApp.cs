using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionLauncher.Apps
{
    public class InstallApp
    {
        public long Id { get; set; }
        public long Version { get; set; }
        public long VersionTime { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public AppType AppType { get; set; }
        public InstallType InstallType { get; set; }
        public String DefaultLocation { get; set; }

        public AppDirectory Directory { get; set; }

        public InstallApp()
        {
            Id = 0;
            Version = 0;
            VersionTime = 0;
            Name = "";
            Description = "";
            AppType = AppType.App;
            InstallType = InstallType.Invalid;
            DefaultLocation = "";

            Directory = new AppDirectory();
        }
    }

    public enum InstallType
    {
        Invalid = 0,
        Steam,
        Custom
    }

    public enum AppType
    {
        App = 0,
        Game,
        Mod
    }
}
