/******************************************************************************
 * Filename    = InstructorPage.xaml.cs
 *
 * Author      = Saarang S
 *
 * Product     = Analyzer
 * 
 * Project     = Dashboard
 *
 * Description = Code behind of a Instructor Page.
 *****************************************************************************/
using ContentPage;
using SessionState;
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
using ViewModel;
using ServerlessFuncUI;
using Logging;
using System.Runtime.CompilerServices;


namespace Dashboard
{
    /// <summary>
    /// Represents the WPF page for the instructor's dashboard.
    /// </summary>
    public partial class InstructorPage : Page
    {
        private readonly ServerPage _contentServerPage;


        /// <summary>
        /// Initializes a new instance of the <see cref="InstructorPage"/> class.
        /// </summary>
        /// <param name="userName">The username of the instructor.</param>
        /// <param name="userId">The user ID of the instructor.</param>
        public InstructorPage( string userName , string userId, string userImage, bool isDark )
        {
            InitializeComponent();
            Unloaded += InstructorPage_Unloaded;
            try
            {
                isDarkMode = true;
                if (!isDark)
                {
                    Resources.Source = (new Uri( "Theme/Light.xaml" , UriKind.Relative ));
                    isDarkMode = false;
                }
                // Create and set up the ViewModel
                InstructorViewModel viewModel = new(userName,userId,userImage);
                DataContext = viewModel;

                // Create and set up the ServerPage
                _contentServerPage = new ServerPage ( viewModel.Communicator, userId);
                ResultFrame.Content = _contentServerPage;

                //Create and ste up the Cloud Page
                SessionsPage _cloudPage = new (userId);
                CloudFrame.Content = _cloudPage;

            }
            catch (Exception exception)
            {
                // Display an error message and shutdown the application on exception
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        private void InstructorPage_Unloaded ( object sender , RoutedEventArgs e )
        {
            Logger.Inform( "[InstructorPage] Unloading" );
            LogoutButton_Click( sender , e );
        }



        /// <summary>
        /// Gets the current Theme
        /// </summary>
        public bool isDarkMode { get; set; }

        /// <summary>
        /// Event handler for the logout button click.
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the AuthenticationPage when the logout button is clicked
            InstructorViewModel? viewModel = DataContext as InstructorViewModel;
            viewModel?.Logout();
            Application.Current.Shutdown();
            //AuthenticationPage authenticationPage = new ();
            //NavigationService?.Navigate( authenticationPage );
        }

        /// <summary>
        /// Event handler for when a student is selected in the list.
        /// </summary>
        private void Student_Selected(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.IsSelected)
            {
                if (item.DataContext is Student clickedStudent)
                {
                    if (clickedStudent.Id != null)
                    {
                        // Set the session ID in the ServerPage when a student is selected
                        _contentServerPage.SetSessionID( clickedStudent.Id );
                    }
                }
            }
        }


        private void ChangeTheme( object sender , RoutedEventArgs e )
        {
            if (isDarkMode)
            {
                Resources.Source = ( new Uri( "Theme/Light.xaml" , UriKind.Relative ) );
                isDarkMode = false;
            }
            else
            {
                Resources.Source = (new Uri("Theme/Dark.xaml", UriKind.Relative));
                isDarkMode = true;
            }
            _contentServerPage.SetDarkMode( isDarkMode );
        }
    }
}
