using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GabionCache.Storage;

namespace GabionCache.Dao
{
    public class StorageDao
    {
        public StorageFile LoadFile(String path)
        {
            StorageFile file = new StorageFile();

            try
            {
                file.Data = File.ReadAllBytes(path);

                file.Size = file.Data.LongLength;
            }
            catch (Exception e)
            {
                file = null;
            }

            return file;
        }

        public bool SaveFile(String path, StorageFile file)
        {
            bool success = true;

            try
            {
                File.WriteAllBytes(path, file.Data);
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }
    }
}
