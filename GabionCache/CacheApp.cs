using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GabionCache.Proxy;
using GabionCache.Settings;
using GabionCache.Storage;

namespace GabionCache
{
    public static class CacheApp
    {
        public static ProxySettings ProxySettings;
        public static StorageSettings StorageSettings;

        public static ReverseProxy Server;

        public static DiskStorage Disk;
        public static MemoryStorage Memory;

        public static void Initialize()
        {
            Disk = new DiskStorage();
            Memory = new MemoryStorage();
        }
    }
}
