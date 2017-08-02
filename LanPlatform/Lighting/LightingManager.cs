using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.DAL;
using GabionPlatform.Models;

namespace GabionPlatform.Lighting
{
    public class LightingManager
    {
        protected AppInstance Instance;
        protected PlatformContext Context;

        public LightingManager(AppInstance instance)
        {
            Instance = instance;
            Context = instance.Context;
        }


    }
}