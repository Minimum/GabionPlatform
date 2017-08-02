using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionInstaller.Installs
{
    public class InstallFile
    {
        public String Name { get; set; }
        public long Size { get; set; }

        public InstallFile()
        {
            Name = "";
            Size = 0;
        }
    }
}
