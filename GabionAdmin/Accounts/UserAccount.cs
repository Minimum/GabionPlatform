using System;
using GabionAdmin.Database;

namespace GabionAdmin.Accounts
{
    public class UserAccount : EditableDatabaseObject
    {
        // Access
        public bool Root { get; set; }
        public UserType Type { get; set; }

        // Information Variables
        public UserGender Gender { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public long Birthday { get; set; }
        public String ContactEmail { get; set; }
        public String ContactPhone { get; set; }
        public String ContactFacebook { get; set; }
        public long ContactSteam { get; set; }

        // Player Variables
        public long TotalEvents { get; set; }
        public long EventOffset { get; set; }
        public long RemoteEvents { get; set; }
        public long LastEvent { get; set; }
        public String DisplayName { get; set; }
        public String CustomUrl { get; set; }
        public long LastActive { get; set; }
        public long Avatar { get; set; }
        public UserVisibility Visibility { get; set; }

        // Award Variables
        public bool AwardsEnabled { get; set; }
        public bool AwardsXpEnabled { get; set; }
        public long AwardsLevel { get; set; }
        public long AwardsXp { get; set; }

        public UserAccount()
        {
            Root = false;
            Type = UserType.Player;

            Gender = UserGender.None;
            FirstName = "";
            LastName = "";
            Birthday = 0;
            ContactEmail = "";
            ContactPhone = "";
            ContactFacebook = "";
            ContactSteam = 0;

            Visibility = UserVisibility.Visible;
            TotalEvents = 0;
            EventOffset = 0;
            RemoteEvents = 0;
            DisplayName = "";
            CustomUrl = "";
            LastEvent = 0;
            LastActive = 0;
            Avatar = 0;
            
            AwardsEnabled = true;
            AwardsXpEnabled = true;
            AwardsLevel = 0;
            AwardsXp = 0;
        }
    }

    public enum UserVisibility
    {
        Visible = 0,
        HiddenFromGuests,
        HiddenFromUsers
    }

    public enum UserType
    {
        Player = 0,          // 0
        Bot                  // 1
    }

    public enum UserGender
    {
        None = 0,
        Male,
        Female,
        Other
    }
}