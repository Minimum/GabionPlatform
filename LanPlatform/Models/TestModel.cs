using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.Models
{
    public class TestModel : EditableDatabaseObject
    {
        public String Name { get; set; }
    }
}