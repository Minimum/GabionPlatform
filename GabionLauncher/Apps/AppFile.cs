using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionLauncher.Apps
{
    public class AppFile : AppObject
    {
        public long Size { get; set; }
        public long Version { get; set; }
        public String Hash { get; set; }

        public AppFile()
        {
            Size = 0;
            Version = 0;
            Hash = "";
        }

        public AppFile(String name)
            : base(name)
        {
            Size = 0;
            Version = 0;
            Hash = "";
        }
    }
}
