using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionCache.Storage
{
    public class VirtualDirectory
    {
        public String Name { get; set; }

        protected Dictionary<String, VirtualDirectory> Directories;  
        protected Dictionary<String, StorageFile> Files;

        public VirtualDirectory()
        {
            Directories = new Dictionary<string, VirtualDirectory>();
            Files = new Dictionary<string, StorageFile>();
        }

        public StorageFile GetFile(String name)
        {
            StorageFile file = null;
            String fileName = name.ToLower();

            lock (Files)
            {
                if (Files.ContainsKey(fileName))
                {
                    file = Files[fileName];
                }
            }

            return file;
        }

        public VirtualDirectory GetDirectory(String name)
        {
            VirtualDirectory dir = null;
            String dirName = name.ToLower();

            lock (Directories)
            {
                if (Directories.ContainsKey(dirName))
                {
                    dir = Directories[dirName];
                }
            }

            return dir;
        }

        public bool AddFile(StorageFile file)
        {
            bool success = false;
            String fileName = file.Name.ToLower();

            lock (Files)
            {
                if (!Files.ContainsKey(fileName))
                {
                    Files.Add(fileName, file);

                    success = true;
                }
            }

            return success;
        }

        public void RemoveFile(StorageFile file)
        {
            lock (Files)
            {
                Files.Remove(file.Name.ToLower());
            }

            return;
        }

        public bool AddDirectory(VirtualDirectory directory)
        {
            bool success = false;
            String dirName = directory.Name.ToLower();

            lock (Directories)
            {
                if (!Directories.ContainsKey(dirName))
                {
                    Directories.Add(dirName, directory);

                    success = true;
                }
            }

            return success;
        }
    }
}
