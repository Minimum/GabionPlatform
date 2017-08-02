using System;
using GabionPlatform.News;
using Newtonsoft.Json;

namespace GabionPlatform.Models
{
    public class NewsStatusModel
    {
        [JsonIgnore]
        public NewsStatus Target { get; set; }

        public long Id { get { return Target.Id; } }

        public String Title { get { return Target.Title; } }

        public String Content { get { return Target.Content; } }

        public NewsStatusModel(NewsStatus target)
        {
            Target = target;
        }
    }
}