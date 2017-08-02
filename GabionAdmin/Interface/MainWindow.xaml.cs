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
using System.Windows.Shapes;

namespace GabionAdmin.Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private enum NavSections
        {
            Default = 0,

            // Home
            Home,
            HomeStatus,
            HomeQuickLinks,
            HomeWeather,

            // Library
            Library,
            LibraryApps,
            LibraryLoaners,

            // Ambience
            Ambience,
            AmbienceMusic,
            AmbienceLighting,

            // Community
            Community,
            CommunityUsers,
            CommunityAwards,
            CommunityEvents,
            CommunityLeaderboards,
            CommunityChat
        }

        private void NavCommunityUsers_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
