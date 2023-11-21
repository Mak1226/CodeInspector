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
using ContentPage;

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

            try
            {
                // Create the ViewModel and set as data context.
                StudentViewModel viewModel = new();
                DataContext = viewModel;

                
            }
            catch (Exception exception)
            {
                // If an exception occurs during ViewModel creation, show an error message and shutdown the application.
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // If a valid NavigationService exists, navigate to the "Login.xaml" page.
            NavigationService?.Navigate(new Uri("AuthenticationPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Event handler for the "IstructorIpTextBox" text changed event.
        /// </summary>
        //private void IstructorIpTextBox_TextChanged(object sender, TextChangedEventArgs e)

        private void InstructorIpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StudentViewModel? viewModel = DataContext as StudentViewModel;
            viewModel?.SetInstructorAddress(InstructorIpTextBox.Text, InstructorPortTextBox.Text);
            viewModel?.SetStudentInfo( StudentNameTextBox.Text , StudentRollTextBox.Text );
        }

        /// <summary>
        /// Event handler for the "Connect" button click.
        /// </summary>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // Show a message box indicating an attempt to connect to the specified IP address and port.
            StudentViewModel? viewModel = DataContext as StudentViewModel;
            viewModel?.ConnectInstructor();
            if(viewModel != null)
            {
                ClientPage clientPage = new(viewModel.Communicator, StudentRollTextBox.Text);
                ContentFrame.Content = clientPage;
            }

        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            //Attempting to disconnect from the instructor
            StudentViewModel? viewModel = DataContext as StudentViewModel;
            viewModel?.DisconnectInstructor();
        }

    }
}
