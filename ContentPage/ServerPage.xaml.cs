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
        private ContentServerViewModel viewModel;

        /// <summary>
        /// Create a server page instance.
        /// Refer <see cref="ServerPage.LoadResultPage"/> on how to change result to each client's
        /// </summary>
        /// <param name="server">Running networking server</param>
        public ServerPage(ICommunicator server)
        {
            InitializeComponent();
            viewModel = new ContentServerViewModel(server);
            DataContext = viewModel;

            LoadResultPage(""); // Load ResultPage initially
            LoadConfigurationPage(); // Optionally, load ConfigurationPage initially
        }

       
        /// <summary>
        /// Load the result page of the given client/session
        /// </summary>
        /// <param name="sessionID">ID of the client</param>
        public void LoadResultPage(string sessionID)
        {
            ResultPage resultPage = new ResultPage(viewModel, sessionID);
            ResultFrame.NavigationService.Navigate(resultPage);
            
        }

        private void LoadConfigurationPage()
        {
            ConfigurationPage configPage = new ConfigurationPage(viewModel);
            ConfigFrame.NavigationService.Navigate(configPage);
            
        }
    }
}

