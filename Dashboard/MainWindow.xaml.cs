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
using Dashboard;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    //private Frame _frame;

    //public AuthenticationViewModel(Frame frame)
    //{
    //    _frame = frame;
    //}
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();

            //try
            //{
            //    // Create the main frame
            //    //Frame mainFrame = new Frame();

            //    //// Set the main window content to the frame
            //    //Content = mainFrame;

            //    //// Create and set up the navigation service
            //    //INavigationService navigationService = new NavigationService(mainFrame);

            //    MainFrame.Content = new Page1();
            //    this.Show();
            //}
            //catch (Exception exception)
            //{
            //    // If an exception occurs during ViewModel creation or navigation, show an error message and shutdown the application.
            //    MessageBox.Show(exception.Message);
            //    Application.Current.Shutdown();
            //}
            InitializeComponent();
            Page entryPage = new Page1();
            MainFrame.Navigate(entryPage);
        }
    }
}


