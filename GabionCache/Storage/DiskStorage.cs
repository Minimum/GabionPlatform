using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GabionCache.Settings;

namespace GabionCache.Storage
{
    public class DiskStorage
    {
        private String DiskPath;

        public DiskStorage()
        {
            DiskPath = "";
        }

        public DiskStorage(StorageSettings settings)
        {
            DiskPath = settings.DiskPath;
        }

        public bool GetFile(String path, StorageFile file)
        {
            bool success = true;

            try
            {
                using (FileStream stream = new FileStream(DiskPath + path + ".data", FileMode.Open))
                {
                    file.Load(stream, 4096);
                }
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public bool SaveFile(String path, StorageFile file)
        {
            bool success = true;

            try
            {
                using (FileStream stream = new FileStream(DiskPath + path + ".data", FileMode.Create))
                {
                    // TODO: long to int - this will be a problem with large files
                    stream.Write(file.Data, 0, (int) file.Size);
                }
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }
    }
}
