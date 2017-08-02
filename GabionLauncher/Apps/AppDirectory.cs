using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionLauncher.Apps
{
    public class AppDirectory : AppObject
    {
        protected Dictionary<String, AppObject> Files; 

        public AppDirectory()
        {
            Files = new Dictionary<String, AppObject>();
        }

        public AppDirectory(String name)
            : base(name)
        {
            Files = new Dictionary<String, AppObject>();
        }

        public List<AppObject> Get()
        {
            return Files.Values.ToList();
        }

        public AppObject Get(String name)
        {
            return Files.FirstOrDefault(s => s.Key.Equals(name, StringComparison.OrdinalIgnoreCase)).Value;
        }

        public bool Add(AppObject file)
        {
            bool success = false;

            if (!Files.ContainsKey(file.Name))
            {
                Files.Add(file.Name, file);

                success = true;
            }

            return success;
        }

        public bool Remove(String name)
        {
            return Files.Remove(name);
        }
    }
}
