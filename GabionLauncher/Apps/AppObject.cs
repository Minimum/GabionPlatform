using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionLauncher.Apps
{
    public abstract class AppObject
    {
        public String Name { get; set; }

        public AppObject()
        {
            Name = "";
        }

        public AppObject(String name)
        {
            Name = name;
        }
    }
}
