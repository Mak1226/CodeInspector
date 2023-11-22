using Content.ViewModel;
using Networking.Communicator;
using System.Windows.Controls;


namespace ContentPage
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// Main page to be loaded for client
    /// </summary>
    public partial class ClientPage : Page
    {
        private readonly ContentClientViewModel _viewModel;
        /// <summary>
        /// Initialses the page
        /// </summary>
        /// <param name="client">The input ICommuncator that can send messages to server</param>
        /// <param name="sessionID">Some unique identifier for this user</param>
        public ClientPage(ICommunicator client, string sessionID)
        {
            InitializeComponent();
            _viewModel = new ContentClientViewModel(client, sessionID);
            DataContext = _viewModel;

            UploadFrame.Navigate(new FileUpload(_viewModel));
            ResultFrame.Navigate(new ResultPage(_viewModel));
        }
    }
}
