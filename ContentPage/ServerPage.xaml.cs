using System.Windows.Controls;
using Content.ViewModel;
using Networking.Communicator;

namespace ContentPage
{
    public partial class ServerPage : Page
    {
        private ContentServerViewModel viewModel;

        public ServerPage(ICommunicator server)
        {
            InitializeComponent();
            viewModel = new ContentServerViewModel(server);
            DataContext = viewModel;

            LoadResultPage(); // Load ResultPage initially
            LoadConfigurationPage(); // Optionally, load ConfigurationPage initially
        }

       

        private void LoadResultPage()
        {
            ResultPage resultPage = new ResultPage(viewModel);
            ResultFrame.NavigationService.Navigate(resultPage);
            
        }

        private void LoadConfigurationPage()
        {
            ConfigurationPage configPage = new ConfigurationPage(viewModel);
            ConfigFrame.NavigationService.Navigate(configPage);
            
        }
    }
}

