using Networking;
using Networking.Communicator;
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

        private void FileUploadButton_Click(object sender, RoutedEventArgs e)
        {
            ICommunicator client = CommunicationFactory.GetCommunicator(false);
            client.Start("localhost", 12345, "TestClient");
            Page uploadPage = new FileUpload(client, "TestClient");
            MainFrame.Navigate(uploadPage);
        }
        private void ResultPageButton_Click(object sender, RoutedEventArgs e)
        {
            ICommunicator server = CommunicationFactory.GetCommunicator(true);
            server.Start(null, null, "server");
            Page resultPage = new ResultPage(server);
            MainFrame.Navigate(resultPage);
        }
    }
}
