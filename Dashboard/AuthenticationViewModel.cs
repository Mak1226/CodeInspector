//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using static System.Net.Mime.MediaTypeNames;
//using System.Threading;
//using System.Windows.Navigation;
//using Dashboard;
//using Dashboard.Authentication;

//namespace Dashboard
//{
    //[INotifyPropertyChanged]
    //internal partial class AuthenticationViewModel
    //{
    //    [ObservableProperty]
    //    private bool? _isButtonEnabled = true;

    //    [RelayCommand]

    //    {
    //        IsButtonEnabled = false;
    //        //await Task.Delay(3000);
    //        IsButtonEnabled = true;

    //        AuthenticationResult authResult = await Authenticator.Authenticate();

    //        NavigationStore navigationStore = new()
    //        {public async Task AuthenticateButton_Click(CancellationToken token)
    //            AuthResult = authResult
    //        };
    //        navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);

    //        var newWindow = new MainWindow
    //        {
    //            DataContext = new MainViewModel(navigationStore)
    //        };
    //        newWindow.Show();


    //        //used null propogation
    //        Application.Current.MainWindow?.Close();
    //    }

//    internal partial class AuthenticationViewModel
//    {
//        private readonly NavigationService ?_navigationService; // Add a NavigationService reference
//        public async Task AuthenticateButton_Click(CancellationToken token)
//        {
//            AuthenticationResult authResult = await Authenticator.Authenticate();

//            if (authResult.IsAuthenticated == true)

//                //have to implement autofill when navigating to the student or instructor views
//                _navigationService?.Navigate(new Uri("Login.xaml", UriKind.Relative));
                
//                else
//                {
//                    MessageBox.Show("Authentication failed. Please try again.");
//                    //show pop up to say error with authorization
//                }
//        }
//    }
//}



using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Dashboard;
using Dashboard.Authentication;
using System.ComponentModel;

namespace Dashboard
{
    internal partial class AuthenticationViewModel : ObservableObject
    {
        private async Task<AuthenticationResult> Authenticate()
        {
            // Perform authentication logic here, return true if authenticated, false otherwise
            AuthenticationResult authResult = await Authenticator.Authenticate();
            return authResult;
        }

        // Event triggered on successful authentication

        [RelayCommand]
        public async Task<AuthenticationResult> AuthenticateButton_Click()
        {
            AuthenticationResult authenticationResult = await Authenticate();
            return authenticationResult;
        }
    }
}


    //navigation.CurrentViewModel = new HomeViewModel(navigationStore);

    //var newWindow = new MainWindow
    //{
    //    DataContext = new MainViewModel(navigationStore)
    //};
    //newWindow.Show();


    ////used null propogation
    //Application.Current.MainWindow?.Close();
//}
//    }
//}