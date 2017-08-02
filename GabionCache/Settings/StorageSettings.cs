using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionCache.Settings
{
    public class StorageSettings
    {
        public long MaxDiskSize { get; set; }
        public long MaxMemorySize { get; set; }
        public String DiskPath { get; set; }

        public StorageSettings()
        {
            MaxDiskSize = 0;
            MaxMemorySize = 0;
            DiskPath = "";
        }
    }
}
