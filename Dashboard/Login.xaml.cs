using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login( string userName , string userEmail , string userPicture )
        {
            InitializeComponent();
            UserName = userName;
            UserId = userEmail;
            DataContext = this; // Set the DataContext to this instance
        }

        public string UserName { get; init; }
        public string UserId { get; init; }

        private void InstructorButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Uri("InstructorPage.xaml", UriKind.Relative));
            }
        }
        private void StudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Uri("StudentView.xaml", UriKind.Relative));
            }
        }
    }
}
