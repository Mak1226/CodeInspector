/******************************************************************************
 * Filename    = StudentPage.xaml.cs
 *
 * Author      = Prayag Krishna
 *
 * Product     = Analyzer
 * 
 * Project     = Dashboard
 *
 * Description = Defines the student page code-behind.
 *****************************************************************************/
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
using ContentPage;
using ViewModel;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        public StudentPage(string name, string id)
        {
            InitializeComponent();

            try
            {
                // Create the ViewModel and set as data context.
                StudentViewModel viewModel = new(name, id);
                DataContext = viewModel;
                //viewModel?.SetStudentInfo( StudentName , StudentId);
            }
            catch (Exception exception)
            {
                // If an exception occurs during ViewModel creation, show an error message and shutdown the application.
                _ = MessageBox.Show( exception.Message );
                Application.Current.Shutdown();
            }
        }

        private void LogoutButton_Click( object sender , RoutedEventArgs e )
        {
            // If a valid NavigationService exists, navigate to the "Login.xaml" page.
            StudentViewModel? viewModel = DataContext as StudentViewModel;

            viewModel?.DisconnectFromInstructor();
            AuthenticationPage authenticationPage = new();
            NavigationService?.Navigate( authenticationPage );
        }

        /// <summary>
        /// Event handler for the "IstructorIpTextBox" text changed event.
        /// </summary>
        private void InstructorIpTextBox_TextChanged( object sender , TextChangedEventArgs e )
        {
            StudentViewModel? viewModel = DataContext as StudentViewModel;
            viewModel?.SetInstructorAddress( InstructorIpTextBox.Text , InstructorPortTextBox.Text );
        }

        /// <summary>
        /// Event handler for the "Connect" button click.
        /// </summary>
        private void ConnectButton_Click( object sender , RoutedEventArgs e )
        {
            // Show a message box indicating an attempt to connect to the specified IP address and port.
            StudentViewModel? viewModel = DataContext as StudentViewModel;

            bool? isConnected = viewModel?.ConnectToInstructor();
            if(isConnected != null && viewModel != null)
            {
                if ( isConnected.Value )
                {
                    ClientPage clientPage = new( viewModel.Communicator , viewModel.StudentRoll );
                    ContentFrame.Content = clientPage;
                }
            }
        }

        private void DisconnectButton_Click( object sender , RoutedEventArgs e )
        {
            //Attempting to disconnect from the instructor
            StudentViewModel? viewModel = DataContext as StudentViewModel;
            viewModel?.DisconnectFromInstructor();
        }
    }
}
