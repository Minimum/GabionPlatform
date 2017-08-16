using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Database;

namespace GabionPlatform.DTO
{
    public abstract class GabionDto
    {
        public long Id { get; set; }

        protected GabionDto()
        {
            Id = 0;
        }

        protected GabionDto(DatabaseObject model)
        {
            Id = model.Id;
        }
    }
}