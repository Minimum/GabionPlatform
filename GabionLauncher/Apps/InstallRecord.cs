using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionLauncher.Apps
{
    public class InstallRecord
    {
        public long App { get; set; }
        public long Version { get; set; }
        public long TimeInstalled { get; set; }

        public AppDirectory Files { get; set; }

        public InstallRecord()
        {
            App = 0;
            Version = 0;
            TimeInstalled = 0;

            Files = new AppDirectory();
        }
    }
}
