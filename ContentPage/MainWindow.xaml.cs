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

        private void FileUploadButton_Click(object sender, RoutedEventArgs e)
        {
            ICommunicator client = CommunicationFactory.GetClient();
            client.Start("localhost", 12399, "TestClient");
            Page uploadPage = new FileUpload(client, "TestClient");
            MainFrame.Navigate(uploadPage);
        }
        private void ResultPageButton_Click(object sender, RoutedEventArgs e)
        {
            ICommunicator server = CommunicationFactory.GetServer();
            server.Start(null, null, ID.GetServerID());
            Page resultPage = new ResultPage(server);
            MainFrame.Navigate(resultPage);
        }
    }
}
