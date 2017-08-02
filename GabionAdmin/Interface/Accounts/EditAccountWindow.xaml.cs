using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GabionAdmin.Accounts;

namespace GabionAdmin.Interface.Accounts
{
    /// <summary>
    /// Interaction logic for EditAccountWindow.xaml
    /// </summary>
    public partial class EditAccountWindow : Window
    {
        public EditAccountWindow()
        {
            InitializeComponent();

            Account = new UserAccount();

            Working = false;
        }

        private UserAccount Account;
        private UserAccount EditedAccount;
        private UserAccount OldAccount;

        private BackgroundWorker Worker;

        private bool Working;

        public void Initialize(UserAccount account)
        {
            Account = account;

            Title = "Account: #" + account.Id + " - " + account.DisplayName;

            // General
            UserIdField.Text = account.Id.ToString();
            AccountTypeField.SelectedIndex = (int) account.Type;
            RootField.IsChecked = account.Root;
            DisplayNameField.Text = account.DisplayName;
            CustomUrlField.Text = account.CustomUrl;
            VisibilityField.SelectedIndex = (int) Visibility.Visible;
            
            // Social
            FirstNameField.Text = account.FirstName;
            LastNameField.Text = account.LastName;
            GenderField.SelectedIndex = (int) account.Gender;
            PhoneField.Text = account.ContactPhone;
            EmailField.Text = account.ContactEmail;
            FacebookField.Text = account.ContactFacebook;
            SteamField.Text = account.ContactSteam.ToString();

            // Events
            TotalEventsField.Text = account.TotalEvents.ToString();
            EventOffsetField.Text = account.EventOffset.ToString();
            RemoteEventsField.Text = account.RemoteEvents.ToString();
            LastEventField.Text = account.LastEvent.ToString();

            // Awards
            AwardEnabledField.IsChecked = account.AwardsEnabled;
            AwardLevelField.Text = account.AwardsLevel.ToString();
            AwardXPField.Text = account.AwardsXp.ToString();
            AwardNextLevelField.Text = (500 * account.AwardsLevel + 1000).ToString();
            AwardXPEnabledField.IsChecked = account.AwardsXpEnabled;

            SetFieldStatus(account, "Old Value");

            Visibility = Visibility.Visible;

            return;
        }

        private void SetFieldStatus(UserAccount account, String prefix)
        {
            // General
            switch (account.Type)
            {
                case UserType.Player:
                    AccountTypeStatus.Content = prefix + ": Player";
                    break;

                case UserType.Bot:
                    AccountTypeStatus.Content = prefix + ": Bot";
                    break;

                default:
                    AccountTypeStatus.Content = "THIS IS A BUG: UNSUPPORTED VALUE";
                    break;
            }

            DisplayNameStatus.Content = prefix + ": " + account.DisplayName;
            CustomUrlStatus.Content = prefix + ": " + account.CustomUrl;

            switch (account.Visibility)
            {
                case UserVisibility.Visible:
                    VisibilityStatus.Content = prefix + ": Visible";
                    break;

                case UserVisibility.HiddenFromGuests:
                    VisibilityStatus.Content = prefix + ": Hidden from guests";
                    break;

                case UserVisibility.HiddenFromUsers:
                    VisibilityStatus.Content = prefix + ": Hidden from users";
                    break;

                default:
                    VisibilityStatus.Content = "THIS IS A BUG: UNSUPPORTED VALUE";
                    break;
            }

            // Social
            FirstNameStatus.Content = prefix + ": " + account.FirstName;
            LastNameStatus.Content = prefix + ": " + account.LastName;

            switch (account.Gender)
            {
                case UserGender.None:
                    GenderStatus.Content = prefix + ": None";
                    break;

                case UserGender.Male:
                    GenderStatus.Content = prefix + ": Male";
                    break;

                case UserGender.Female:
                    GenderStatus.Content = prefix + ": Female";
                    break;

                case UserGender.Other:
                    GenderStatus.Content = prefix + ": Other";
                    break;

                default:
                    GenderStatus.Content = "THIS IS A BUG: UNSUPPORTED VALUE";
                    break;
            }

            PhoneStatus.Content = prefix + ": " + account.ContactPhone;
            EmailStatus.Content = prefix + ": " + account.ContactEmail;
            FacebookStatus.Content = prefix + ": " + account.ContactFacebook;
            SteamStatus.Content = prefix + ": " + account.ContactSteam;

            // Events
            TotalEventsStatus.Content = prefix + ": " + account.TotalEvents;
            EventOffsetStatus.Content = prefix + ": " + account.EventOffset;
            RemoteEventsStatus.Content = prefix + ": " + account.RemoteEvents;
            LastEventStatus.Content = prefix + ": " + account.LastEvent;

            // Awards
            AwardLevelStatus.Content = prefix + ": " + account.AwardsLevel;
            AwardXPStatus.Content = prefix + ": " + account.AwardsXp;
        }

        private void SetFieldEditing(bool freeze)
        {
            // General
            AccountTypeField.IsEnabled = !freeze;
            RootField.IsEnabled = !freeze;
            DisplayNameField.IsEnabled = !freeze;
            CustomUrlField.IsEnabled = !freeze;
            VisibilityField.IsEnabled = !freeze;

            // Social
            FirstNameField.IsEnabled = !freeze;
            LastNameField.IsEnabled = !freeze;
            GenderField.IsEnabled = !freeze;
            PhoneField.IsEnabled = !freeze;
            EmailField.IsEnabled = !freeze;
            FacebookField.IsEnabled = !freeze;
            SteamField.IsEnabled = !freeze;

            // Events
            TotalEventsField.IsEnabled = !freeze;
            EventOffsetField.IsEnabled = !freeze;
            RemoteEventsField.IsEnabled = !freeze;
            LastEventField.IsEnabled = !freeze;

            // Awards
            AwardEnabledField.IsEnabled = !freeze;
            AwardLevelField.IsEnabled = !freeze;
            AwardXPField.IsEnabled = !freeze;
            AwardNextLevelField.IsEnabled = !freeze;
            AwardXPEnabledField.IsEnabled = !freeze;

            return;
        }

        private UserAccount GenerateEditedAccount()
        {
            UserAccount account = new UserAccount();

            // General
            long id;
            account.Id = Int64.TryParse(UserIdField.Text, out id) ? id : 0;

            account.Version = (Account != null) ? Account.Version : 0;
            account.Type = (UserType) AccountTypeField.SelectedIndex;
            account.Root = RootField.IsChecked != null && RootField.IsChecked == true;
            account.DisplayName = DisplayNameField.Text;
            account.CustomUrl = CustomUrlField.Text;
            account.Visibility = (UserVisibility) VisibilityField.SelectedIndex;

            // Social
            account.FirstName = FirstNameField.Text;
            account.LastName = LastEventField.Text;
            account.Gender = (UserGender) GenderField.SelectedIndex;
            account.ContactPhone = PhoneField.Text;
            account.ContactEmail = EmailField.Text;
            account.ContactFacebook = FacebookField.Text;

            long contactSteam;
            account.ContactSteam = Int64.TryParse(SteamField.Text, out contactSteam) ? contactSteam : 0;

            // Events
            long totalEvents;
            account.TotalEvents = Int64.TryParse(TotalEventsField.Text, out totalEvents) ? totalEvents : 0;

            long eventOffset;
            account.EventOffset = Int64.TryParse(EventOffsetField.Text, out eventOffset) ? eventOffset : 0;

            long remoteEvents;
            account.RemoteEvents = Int64.TryParse(RemoteEventsField.Text, out remoteEvents) ? remoteEvents : 0;

            long lastEvent;
            account.LastEvent = Int64.TryParse(LastEventField.Text, out lastEvent) ? lastEvent : 0;

            // Awards
            account.AwardsEnabled = AwardEnabledField.IsChecked != null && AwardEnabledField.IsChecked == true;

            long awardLevel;
            account.AwardsLevel = Int64.TryParse(AwardLevelField.Text, out awardLevel) ? awardLevel : 0;

            long awardXp;
            account.AwardsXp = Int64.TryParse(AwardXPField.Text, out awardXp) ? awardXp : 0;

            account.AwardsXpEnabled = AwardXPEnabledField.IsChecked != null && AwardXPEnabledField.IsChecked == true;

            return account;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if(!Working)
                Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Working)
            {
                Working = true;

                SetFieldEditing(true);

                EditedAccount = GenerateEditedAccount();

                WebClient client = new WebClient();

                client.UploadStringAsync();

                StatusLabel.Content = "Submitting edits to API...";
                StatusLabel.Visibility = Visibility.Visible;
            }
        }

        private void AccountTypeField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserType type = (UserType) AccountTypeField.SelectedIndex;

            if (Account != null && Account.Type != type)
            {
                if(AccountTypeStatus != null)
                    AccountTypeStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (AccountTypeStatus != null)
                    AccountTypeStatus.Visibility = Visibility.Hidden;
            }
        }

        private void DisplayNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.DisplayName.Equals(DisplayNameField.Text, StringComparison.Ordinal))
            {
                if(DisplayNameStatus != null)
                    DisplayNameStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (DisplayNameStatus != null)
                    DisplayNameStatus.Visibility = Visibility.Hidden;
            }
        }

        private void VisibilityField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserVisibility visibility = (UserVisibility) VisibilityField.SelectedIndex;

            if (Account != null && Account.Visibility != visibility)
            {
                if(VisibilityStatus != null)
                    VisibilityStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (VisibilityStatus != null)
                    VisibilityStatus.Visibility = Visibility.Hidden;
            }
        }

        private void CustomUrlField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.CustomUrl.Equals(CustomUrlField.Text, StringComparison.OrdinalIgnoreCase))
            {
                if(CustomUrlStatus != null)
                    CustomUrlStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (CustomUrlStatus != null)
                    CustomUrlStatus.Visibility = Visibility.Hidden;
            }
        }

        private void FirstNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.FirstName.Equals(FirstNameField.Text, StringComparison.Ordinal))
            {
                if(FirstNameStatus != null)
                    FirstNameStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (FirstNameStatus != null)
                    FirstNameStatus.Visibility = Visibility.Hidden;
            }
        }

        private void LastNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.LastName.Equals(LastNameField.Text, StringComparison.Ordinal))
            {
                if (LastNameStatus != null)
                    LastNameStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (LastNameStatus != null)
                    LastNameStatus.Visibility = Visibility.Hidden;
            }
        }

        private void GenderField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserGender gender = (UserGender) GenderField.SelectedIndex;

            if (Account != null && Account.Gender != gender)
            {
                if(GenderStatus != null)
                    GenderStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (GenderStatus != null)
                    GenderStatus.Visibility = Visibility.Hidden;
            }
        }

        private void PhoneField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.ContactPhone.Equals(PhoneField.Text, StringComparison.OrdinalIgnoreCase))
            {
                if(PhoneStatus != null)
                    PhoneStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (PhoneStatus != null)
                    PhoneStatus.Visibility = Visibility.Hidden;
            }
        }

        private void EmailField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.ContactEmail.Equals(EmailField.Text, StringComparison.OrdinalIgnoreCase))
            {
                if (EmailStatus != null)
                    EmailStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (EmailStatus != null)
                    EmailStatus.Visibility = Visibility.Hidden;
            }
        }

        private void FacebookField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Account != null && !Account.ContactFacebook.Equals(FacebookField.Text, StringComparison.OrdinalIgnoreCase))
            {
                if (FacebookStatus != null)
                    FacebookStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (FacebookStatus != null)
                    FacebookStatus.Visibility = Visibility.Hidden;
            }
        }

        private void SteamField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long steamId;

            if (!Int64.TryParse(SteamField.Text, out steamId))
            {
                steamId = 0;
            }

            if (Account != null && Account.ContactSteam != steamId)
            {
                if (SteamStatus != null)
                    SteamStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (SteamStatus != null)
                    SteamStatus.Visibility = Visibility.Hidden;
            }
        }

        private void TotalEventsField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long totalEvents;

            if (!Int64.TryParse(TotalEventsField.Text, out totalEvents))
            {
                totalEvents = 0;
            }

            if (Account != null && Account.TotalEvents != totalEvents)
            {
                if (TotalEventsStatus != null)
                    TotalEventsStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (TotalEventsStatus != null)
                    TotalEventsStatus.Visibility = Visibility.Hidden;
            }
        }

        private void EventOffsetField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long eventOffset;

            if (!Int64.TryParse(EventOffsetField.Text, out eventOffset))
            {
                eventOffset = 0;
            }

            if (Account != null && Account.EventOffset != eventOffset)
            {
                if (EventOffsetStatus != null)
                    EventOffsetStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (EventOffsetStatus != null)
                    EventOffsetStatus.Visibility = Visibility.Hidden;
            }
        }

        private void RemoteEventsField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long remoteEvents;

            if (!Int64.TryParse(RemoteEventsField.Text, out remoteEvents))
            {
                remoteEvents = 0;
            }

            if (Account != null && Account.RemoteEvents != remoteEvents)
            {
                if (RemoteEventsStatus != null)
                    RemoteEventsStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (RemoteEventsStatus != null)
                    RemoteEventsStatus.Visibility = Visibility.Hidden;
            }

        }

        private void LastEventField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long lastEvent;

            if (!Int64.TryParse(LastEventField.Text, out lastEvent))
            {
                lastEvent = 0;
            }

            if (Account != null && Account.LastEvent != lastEvent)
            {
                if (LastEventStatus != null)
                    LastEventStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (LastEventStatus != null)
                    LastEventStatus.Visibility = Visibility.Hidden;
            }
        }

        private void AwardLevelField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long awardLevel;

            if (!Int64.TryParse(AwardLevelField.Text, out awardLevel))
            {
                awardLevel = 0;
            }

            if (Account != null && Account.AwardsLevel != awardLevel)
            {
                if (AwardLevelStatus != null)
                    AwardLevelStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (AwardLevelStatus != null)
                    AwardLevelStatus.Visibility = Visibility.Hidden;
            }
        }

        private void AwardXPField_TextChanged(object sender, TextChangedEventArgs e)
        {
            long awardXP;

            if (!Int64.TryParse(AwardXPField.Text, out awardXP))
            {
                awardXP = 0;
            }

            if (Account != null && Account.AwardsXp != awardXP)
            {
                if (AwardXPStatus != null)
                    AwardXPStatus.Visibility = Visibility.Visible;
            }
            else
            {
                if (AwardXPStatus != null)
                    AwardXPStatus.Visibility = Visibility.Hidden;
            }
        }

        
    }
}
