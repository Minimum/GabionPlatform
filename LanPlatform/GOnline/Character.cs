using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.GOnline
{
    public class Character : EditableDatabaseObject
    {
        [NotMapped]
        public List<CharacterSkin> Skins { get; set; }
    }
}