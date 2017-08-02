using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionCache.Storage
{
    public abstract class StorageType
    {
        public abstract bool FileExists(String path);
        public abstract StorageFile GetFile(String path);
    }
}
