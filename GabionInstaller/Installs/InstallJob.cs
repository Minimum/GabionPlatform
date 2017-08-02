using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionInstaller.Installs
{
    public class InstallJob
    {
        private Queue<InstallFile> Files;

        private InstallApp App;



        public InstallJob(InstallApp app)
        {
            App = app;

            Files = new Queue<InstallFile>();
        }

        public void Initialize()
        {
            
        }
    }
}
