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

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Entry.xaml
    /// </summary>
    public partial class Entry : Page
    {
        public Entry()
        {
            InitializeComponent();

            try
            {
                // Create the ViewModel and set as data context.
                DashboardViewModel viewModel = new();
                DataContext = viewModel;
            }
            catch (Exception exception)
            {
                _ = MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
            }
        }
    }
}