using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionInstaller.Installs
{
    public class InstallApp
    {
        public long Id { get; set; }

        public String Title { get; set; }

        public String Path { get; set; }
        public InstallFolder Files { get; set; }

        public List<InstallTask> PreInstallTasks { get; set; }
        public List<InstallTask> PostInstallTasks { get; set; }

        public InstallApp()
        {
            
        }
    }
}
