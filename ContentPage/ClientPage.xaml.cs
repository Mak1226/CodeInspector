using Content.ViewModel;
using Networking.Communicator;
using System.Windows.Controls;


namespace ContentPage
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private ContentClientViewModel viewModel;
        public ClientPage(ICommunicator client, string sessionID)
        {
            InitializeComponent();
            viewModel = new ContentClientViewModel(client, sessionID);
            DataContext = viewModel;

            UploadFrame.Navigate(new FileUpload(viewModel));
            ResultFrame.Navigate(new ResultPage(viewModel));
        }
    }
}
