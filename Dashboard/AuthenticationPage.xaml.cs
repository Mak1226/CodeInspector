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
                // Create the ViewModel and set as data context.
                AuthenticationViewModel viewModel = new();
                DataContext = viewModel;

            }
            catch (Exception exception)
            {
                // If an exception occurs during ViewModel creation, show an error message and shutdown the application.
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        private void ShowErrorWindow(string errorMessage)
{
    var errorWindow = new ErrorWindow(errorMessage);
    errorWindow.ShowDialog();
}

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationViewModel viewModel = (AuthenticationViewModel)DataContext;
            try
            {

                AuthenticationResult authenticationResult = await viewModel.AuthenticateButton_Click();

                Debug.WriteLine("Printing from page");
                Debug.WriteLine(authenticationResult.UserName);
                Debug.WriteLine(authenticationResult.UserEmail);
                Debug.WriteLine(authenticationResult.UserImage);

                Application.Current.MainWindow.Activate();

                var loginPage = new Login(authenticationResult.UserName, authenticationResult.UserEmail, authenticationResult.UserImage);
                NavigationService?.Navigate(loginPage);
            }
            catch (Exception ex)
            {
                ShowErrorWindow("Login request using OAuth cancelled before completion, try again!");
                Application.Current.Shutdown();
            }

        }
    }
}
