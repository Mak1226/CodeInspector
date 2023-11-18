using System.Windows.Controls;
using Content.ViewModel;
using Networking.Communicator;

namespace ContentPage
{
    /// <summary>
    /// Logic for Server main page
    /// </summary>
    public partial class ServerPage : Page
    {
        private readonly ContentServerViewModel _viewModel;

        /// <summary>
        /// Create a server page instance.
        /// Refer <see cref="SetSessionID"/> on how to change result to each client's
        /// </summary>
        /// <param name="server">Running networking server</param>
        public ServerPage(ICommunicator server)
        {
            InitializeComponent();
            _viewModel = new ContentServerViewModel(server);
            DataContext = _viewModel;

            LoadResultPage(); // Load ResultPage initially
            LoadConfigurationPage(); // Optionally, load ConfigurationPage initially
        }

        /// <summary>
        /// Set the session/client ID that the server is currently viewing and update result tabs
        /// 
        /// Note that this function has to be called first for server to show any result
        /// </summary>
        /// <param name="sessionID">Session ID or Client ID</param>
        public void SetSessionID(string sessionID)
        {
            _viewModel.SetSessionID(sessionID);
        }

       
        private void LoadResultPage()
        {
            ResultPage resultPage = new (_viewModel);
            ResultFrame.NavigationService.Navigate(resultPage);
            
        }

        private void LoadConfigurationPage()
        {
            ConfigurationPage configPage = new (_viewModel);
            ConfigFrame.NavigationService.Navigate(configPage);
            
        }
    }
}

