using GabionPlatform.Database;

namespace GabionPlatform.Apps
{
    public class LoanerApp : EditableDatabaseObject
    {
        public long Account { get; set; }
        public long App { get; set; }

        public LoanerApp()
        {
            Account = 0;
            App = 0;
        }
    }
}