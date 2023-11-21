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
using Networking.Utils;

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
            InstructorPage instructorPage = new( );
            NavigationService?.Navigate( instructorPage );
        }
            }
        }
            StudentPage studentPage = new(UserName,UserId);
            NavigationService?.Navigate( studentPage );
                this.NavigationService.Navigate(new Uri("StudentView.xaml", UriKind.Relative));
            }
        }
    }
}
