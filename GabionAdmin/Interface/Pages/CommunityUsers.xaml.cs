using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GabionAdmin.Accounts;
using GabionAdmin.Interface.Accounts;

namespace GabionAdmin.Interface.Pages
{
    /// <summary>
    /// Interaction logic for CommunityUsers.xaml
    /// </summary>
    public partial class CommunityUsers : UserControl
    {
        public CommunityUsers()
        {
            InitializeComponent();

            Accounts = new List<UserAccount>();

            PopulateAccounts();
        }

        public List<UserAccount> Accounts;

        private void PopulateAccounts()
        {
            UserAccount account = new UserAccount();

            account.Id = 1;
            account.DisplayName = "Minimum";
            Accounts.Add(account);

            account = new UserAccount();
            account.Id = 2;
            account.DisplayName = "Baron";
            Accounts.Add(account);

            account = new UserAccount();
            account.Id = 5;
            account.DisplayName = "Doritos";
            Accounts.Add(account);

            account = new UserAccount();
            account.Id = 20;
            account.DisplayName = "Kopinski";
            Accounts.Add(account);

            account = new UserAccount();
            account.Id = 1337;
            account.DisplayName = "MOAR BEER";
            Accounts.Add(account);

            UserTable.DataContext = Accounts;
        }

        private void Search()
        {
            
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchTip.Visibility = (SearchBox.Text.Length > 0) ? Visibility.Hidden : Visibility.Visible;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Search();
            }
        }

        private void UserTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void UserTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if (UserTable.SelectedItem != null)
                {
                    EditAccountWindow window = new EditAccountWindow();

                    window.Initialize((UserAccount) UserTable.SelectedItem);
                }
            }
            
            
        }

        private void UserTable_Click(object sender, RoutedEventArgs e)
        {
            if (UserTable.SelectedItem != null)
            {
                EditAccountWindow window = new EditAccountWindow();

                window.Initialize((UserAccount) UserTable.SelectedItem);
            }
        }
    }
}
