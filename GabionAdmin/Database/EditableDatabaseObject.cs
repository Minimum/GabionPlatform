using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GabionAdmin.Database
{
    public abstract class EditableDatabaseObject : DatabaseObject
    {
        [JsonIgnore]
        public byte[] RowVersion { get; set; }

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