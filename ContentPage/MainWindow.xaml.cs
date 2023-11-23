using Networking.Communicator;
using Networking.Utils;
using System.Windows;
using System.Windows.Controls;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileUploadButtonClick(object sender, RoutedEventArgs e)
        {
            ICommunicator client = CommunicationFactory.GetClient();
            client.Start("localhost", 12399, "TestClient", "Content");
            Page clientPage = new ClientPage(client, "TestClient");
            MainFrame.Navigate(clientPage);

            // Hide the buttons
            FileUploadButton.Visibility = Visibility.Collapsed;
            ResultPageButton.Visibility = Visibility.Collapsed;
        }
        private void ClientPageButtonClick(object sender, RoutedEventArgs e)
        {
            ICommunicator server = CommunicationFactory.GetServer();
// <<<<<<< master
            server.Start(null, null, Id.GetServerId(), "Content");
            ServerPage serverPage = new (server, "TestServer" );
// =======
//             server.Start(null, null, ID.GetServerID(), "Content");
//             ServerPage serverPage = new (server, "TestServer");
//>>>>>>> master
            MainFrame.Navigate(serverPage);

            // TestClient by default
            serverPage.SetSessionID("TestClient");

            // Hide the buttons
            ResultPageButton.Visibility = Visibility.Collapsed;
            FileUploadButton.Visibility = Visibility.Collapsed;
        }
    }
}
