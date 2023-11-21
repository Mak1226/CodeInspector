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
//using System.Windows.Forms;
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
        public InstructorPage()
        {
            InitializeComponent();

            try
            {
                InstructorViewModel viewModel = new();
                DataContext = viewModel;

                ServerPage ContentserverPage = new(viewModel.Communicator);
                ResultFrame.Content = ContentserverPage;
            }
            catch (Exception exception)
            {
                // If an exception occurs during ViewModel creation, show an error message and shutdown the application.
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                // If a valid NavigationService exists, navigate to the "Login.xaml" page.
                this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
            }
        }

        private void Student_Selected(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                //Do your stuff
                var clickedStudent = item.DataContext as Student;

                if (clickedStudent != null)
                {
                    Debug.WriteLine($"Clicked {clickedStudent.Id}");
                }
            }
        }
    }
}
