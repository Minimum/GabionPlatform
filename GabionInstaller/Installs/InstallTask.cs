using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionInstaller.Installs
{
    public abstract class InstallTask
    {
        public abstract bool Run(InstallJob job);
    }
}
