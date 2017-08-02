using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace GabionPlatform.Database
{
    public abstract class EditableDatabaseObject : DatabaseObject
    {
        [JsonIgnore]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [NotMapped]
        public long Version
        {
            get
            {
                if (RowVersion != null)
                {
                    return BitConverter.ToInt64(RowVersion, 0);
                }
                else
                {
                    return 0;
                }
            }

            set { RowVersion = BitConverter.GetBytes(value); }
        }

        public EditableDatabaseObject()
        {
            RowVersion = null;
        }
    }
}