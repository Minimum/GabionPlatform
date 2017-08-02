using System;
using GabionPlatform.Content;
using Newtonsoft.Json;

namespace GabionPlatform.Models
{
    public class ContentItemModel
    {
        [JsonIgnore]
        public ContentItem Target { get; set; }

        public long Id
        {
            get { return Target.Id; }
        }

        public long Owner
        {
            get { return Target.Owner; }
        }

        public String Hash
        {
            get { return Target.Hash; }
        }

        public String Filename
        {
            get { return Target.Filename; }
        }

        public long Size
        {
            get { return Target.Size; }
        }

        public ContentItemModel(ContentItem item)
        {
            Target = item;
        }
    }
}