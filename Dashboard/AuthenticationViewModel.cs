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
