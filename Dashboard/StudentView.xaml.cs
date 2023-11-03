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
using ViewModel;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : Page
    {
        /// <summary>
        /// Constructor for the StudentView page.
        /// </summary>
        public StudentView()
        {
            InitializeComponent();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                // If a valid NavigationService exists, navigate to the "Login.xaml" page.
                this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// Event handler for the "IstructorIpTextBox" text changed event.
        /// </summary>
        //private void IstructorIpTextBox_TextChanged(object sender, TextChangedEventArgs e)

        private void InstructorIpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = new DashboardViewModel(); // Create an instance of DashboardViewModel
            viewModel.SetInstructorAddress(InstructorIpTextBox.Text, InstructorPortTextBox.Text);
            viewModel.SetStudentInfo(StudentNameTextBox.Text, StudentNameTextBox.Text);
        }

        /// <summary>
        /// Event handler for the "Connect" button click.
        /// </summary>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // Show a message box indicating an attempt to connect to the specified IP address and port.
            MessageBox.Show("Trying to connect to "+InstructorIpTextBox.Text+" : "+ InstructorPortTextBox.Text);
        }
    }
}
