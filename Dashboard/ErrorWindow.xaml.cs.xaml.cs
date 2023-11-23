/******************************************************************************
 * Filename    = ErrorWindow.xaml.cs.xaml.cs
 *
 * Author      = Prayag Krishna
 *
 * Product     = Analyzer
 * 
 * Project     = ViewModel
 *
 * Description = Pops up the custom Error window.
 *****************************************************************************/
using System;
using System.Timers;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(string errorMessage)
        {
            InitializeComponent();
            ErrorMessage.Text = errorMessage;

            // Start the timer on window load
            Loaded += ErrorWindow_Loaded;
        }

        private void ErrorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Start the timer when the window is loaded
            Timer t = new Timer();
            t.Interval = 4000; // 4 seconds to see the window
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Close the window after the timer interval
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }
    }
}
