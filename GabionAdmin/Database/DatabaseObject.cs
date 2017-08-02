namespace GabionAdmin.Database
{
    public abstract class DatabaseObject
    {
        public long Id { get; set; }

        public DatabaseObject()
        {
            Id = 0;
        }
    }

    
}