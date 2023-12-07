/******************************************************************************
 * Filename    = AuthenticationPage.xaml.cs
 *
 * Author      = Sushma Kamuju
 *
 * Product     = Analyzer
 * 
 * Project     = Dashboard
 *
 * Description = Implements authentication.
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Logging;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
        public AuthenticationPage()
        {
            InitializeComponent();

            try
            {
                Logger.Inform( "[Authenticator Page] Initialized" );
                AuthenticationViewModel viewModel = new();
                DataContext = viewModel;
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

        private async void Login_Click( object sender , RoutedEventArgs e )
        {
            AuthenticationViewModel viewModel = (AuthenticationViewModel)DataContext;

            try
            {
                AuthenticationResult authenticationResult = await viewModel.AuthenticateButton_Click();

                // Log authentication information
                Logger.Inform( "User authenticated successfully:" );
                Logger.Inform( $"UserName: {authenticationResult.UserName}" );
                Logger.Inform( $"UserEmail: {authenticationResult.UserEmail}" );
                Logger.Inform( $"UserImage: {authenticationResult.UserImage}" );

                Application.Current.MainWindow.Activate();

                var loginPage = new Login( authenticationResult.UserName , authenticationResult.UserEmail , authenticationResult.UserImage );
                NavigationService?.Navigate( loginPage );
            }
            catch (Exception ex)
            {
                Logger.Error( $"Login request using OAuth failed: {ex.Message}" );
                ShowErrorAndShutdown( "Login request using OAuth cancelled before completion, try again!" );
            }
        }
    }
}
