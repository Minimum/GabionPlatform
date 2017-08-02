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

namespace GabionAdmin.Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            WarningBrush = new SolidColorBrush(Color.FromArgb(255, 200, 40, 40));
            NormalBrush = new SolidColorBrush(Color.FromArgb(255, 94, 94, 94));
        }

        private readonly SolidColorBrush WarningBrush;
        private readonly SolidColorBrush NormalBrush;

        private void AttemptLogin()
        {
            if (UsernameField.Text.Length > 0 &&
                    PasswordField.Password.Length > 0 &&
                    AddressField.Text.Length > 0)
            {
                // TODO: attempt login
            }

            return;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                AttemptLogin();
            }
        }

        private void UsernameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UsernameField.Text.Length < 1)
            {
                UsernameField.BorderBrush = WarningBrush;
            }
            else
            {
                UsernameField.BorderBrush = NormalBrush;
            }
        }

        private void PasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordField.Password.Length < 1)
            {
                PasswordField.BorderBrush = WarningBrush;
            }
            else
            {
                PasswordField.BorderBrush = NormalBrush;
            }
        }

        private void AddressField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddressField.Text.Length < 1)
            {
                AddressField.BorderBrush = WarningBrush;
            }
            else
            {
                AddressField.BorderBrush = NormalBrush;
            }
        }
    }
}
