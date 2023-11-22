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

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for InstructorPage.xaml
    /// </summary>
    public partial class InstructorPage : Page
    {
        private readonly ServerPage _contentServerPage;
        public InstructorPage( string userName , string userId )
        {
            InitializeComponent();
            try
            {

                InstructorViewModel viewModel = new(null,userName,userId);
                DataContext = viewModel;

                _contentServerPage = new ServerPage ( viewModel.Communicator);
                ResultFrame.Content = _contentServerPage;
            }
            catch (Exception exception)
            {
                // If an exception occurs during ViewModel creation, show an error message and shutdown the application.
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        private void InstructorPage_Unloaded( object sender , RoutedEventArgs e )
        {
            
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // If a valid NavigationService exists, navigate to the "Login.xaml" page.
            NavigationService?.Navigate( new Uri( "AuthenticationPage.xaml" , UriKind.Relative ) );
        }

        private void Student_Selected(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.IsSelected)
            {
                if (item.DataContext is Student clickedStudent)
                {
                    if (clickedStudent.Id != null)
                    {
                        _contentServerPage.SetSessionID( clickedStudent.Id );
                    }
                }
            }
        }
    }
}
