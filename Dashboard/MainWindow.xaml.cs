/******************************************************************************
 * Filename    = MainWindow.xaml.cs
 *
 * Author      = Aravind Somaraj
 *
 * Product     = Analyzer
 * 
 * Project     = Dashboard
 *
 * Description = Defines the Main window is the intermediate start page.
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
using Dashboard;

namespace Dashboard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Page entryPage = new AuthenticationPage();
            MainFrame.Navigate(entryPage);
        }
    }
}


