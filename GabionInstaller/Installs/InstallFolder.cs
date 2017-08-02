using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionInstaller.Installs
{
    public class InstallFolder
    {
        public Dictionary<String, InstallFolder> Folders { get; set; } 
        public Dictionary<String, InstallFile> Files { get; set; }

        public InstallFolder()
        {
            Folders = new Dictionary<String, InstallFolder>();
            Files = new Dictionary<String, InstallFile>();
        }
    }
}
