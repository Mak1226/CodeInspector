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
using Logging;
using System.Runtime.CompilerServices;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        public StudentPage(string name, string id, string userImage, string insIP, string insPort )
        {
            InitializeComponent();

            try
            {
                Logger.Inform( "[Student Page] Initialized" );
                StudentViewModel viewModel = new( name , id , userImage );
                DataContext = viewModel;

                InstructorIpTextBox.Text = insIP;
                InstructorPortTextBox.Text = insPort;

                viewModel?.SetInstructorAddress( insIP , insPort );
                bool? isConnected = viewModel?.ConnectInstructor();
                if (isConnected != null && viewModel != null)
                {
                    if (isConnected.Value)
                    {
                        ClientPage clientPage = new( viewModel.Communicator , viewModel.StudentRoll );
                        ContentFrame.Content = clientPage;
                    }
                }
                Logger.Inform($"[StudentPage] Created viewModel {RuntimeHelpers.GetHashCode( viewModel )}");
                //viewModel?.SetStudentInfo( StudentName , StudentId);
            }
            catch (Exception exception)
            {
                Logger.Error( $"Exception during ViewModel creation: {exception.Message}" );
                ShowErrorAndShutdown( exception.Message );
            }
        }

        private void ShowErrorAndShutdown( string errorMessage )
        {
            Logger.Error( $"Application shutdown due to error: {errorMessage}" );
            MessageBox.Show( errorMessage );
            Application.Current.Shutdown();
        }

        private void LogoutButton_Click( object sender , RoutedEventArgs e )
        {
            StudentViewModel viewModel = DataContext as StudentViewModel;
            viewModel?.DisconnectInstructor();
            NavigationService?.Navigate( new Uri( "AuthenticationPage.xaml" , UriKind.Relative ) );

            Logger.Inform( "User logged out" );
        }

        /// <summary>
        /// Event handler for the "IstructorIpTextBox" text changed event.
        /// </summary>
        /**
        private void InstructorIpTextBox_TextChanged( object sender , TextChangedEventArgs e )
        {
            StudentViewModel viewModel = DataContext as StudentViewModel;
            viewModel?.SetInstructorAddress( InstructorIpTextBox.Text , InstructorPortTextBox.Text );

            Logger.Inform( $"Instructor IP changed to: {InstructorIpTextBox.Text}, Port: {InstructorPortTextBox.Text}" );
        }

        private void ConnectButton_Click( object sender , RoutedEventArgs e )
        {
            StudentViewModel viewModel = DataContext as StudentViewModel;

            bool? isConnected = viewModel?.ConnectInstructor();
            if (isConnected != null && viewModel != null)
            {
                if (isConnected.Value)
                {
                    ClientPage clientPage = new( viewModel.Communicator , viewModel.StudentRoll );
                    ContentFrame.Content = clientPage;
                    Logger.Inform( "Connected to instructor" );
                }
            }
        }
        **/
        private void DisconnectButton_Click( object sender , RoutedEventArgs e )
        {
            StudentViewModel viewModel = DataContext as StudentViewModel;
            viewModel?.DisconnectInstructor();

            Logger.Inform( "Disconnected from instructor" );
        }
    }
}
