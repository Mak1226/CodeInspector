using System;
using System.Net;
using System.Net.Sockets;
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
using ViewModel;
using ContentPage;
using ServerlessFuncUI;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Entry.xaml
    /// </summary>
    public partial class Entry : Page
    {
        /// <summary>
        /// Constructor for the Entry page.
        /// </summary>
        public Entry()
        {
            InitializeComponent();

            try
            {
                // Create the ViewModel and set as data context.
                InstructorViewModel viewModel = new();
                DataContext = viewModel;

                ServerPage serverPage = new(viewModel.Communicator);
                ResultFrame.Content = serverPage;

            }
            catch (Exception exception)
            {
                // If an exception occurs during ViewModel creation, show an error message and shutdown the application.
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Event handler for the "Logout" button click.
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                // If a valid NavigationService exists, navigate to the "Login.xaml" page.
                this.NavigationService.Navigate(new Uri( "AuthenticationPage.xaml" , UriKind.Relative));
            }
        }
    }
}
