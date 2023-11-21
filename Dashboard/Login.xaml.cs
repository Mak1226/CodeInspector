using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page, INotifyPropertyChanged
    {
        public Login(string userName, string userEmail, string userPicture)
        {
            InitializeComponent();
            _name = userName;
            UserName = userName;
            _userId = userEmail;
            UserId = userEmail;
            string picture = userPicture;
            DataContext = this; // Set the DataContext to this instance
        }

        private string _name;
        public string UserName
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InstructorButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Uri("InstructorPage.xaml", UriKind.Relative));
            }
        }
        private void StudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Uri("StudentView.xaml", UriKind.Relative));
            }
        }
    }
}
