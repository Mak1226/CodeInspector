/******************************************************************************
 * Filename    = FileUpload.xaml.cs
 * 
 * Author      = Sreelakshmi
 *
 * Product     = Analyzer
 * 
 * Project     = ContentPage
 *
 * Description = This file contains the code-behind for the MainWindow.xaml.
 *                  This is for testing by content team
 *****************************************************************************/

using Networking.Communicator;
using Networking.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Trace.WriteLine( "Initializing MainWindow" );
            InitializeComponent();
        }
        /// <summary>
        /// Event handler for the file upload button click.
        /// Initializes a client communicator, starts communication, navigates to the FileUpload page, and hides buttons.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void FileUploadButtonClick(object sender, RoutedEventArgs e)
        {
            ICommunicator client = CommunicationFactory.GetClient();
            client.Start("localhost", 12399, "TestClient", "Content");
            Trace.WriteLine( "Client started: TestClient" );
            Page clientPage = new ClientPage(client, "TestClient");
            MainFrame.Navigate(clientPage);

            // Hide the buttons
            FileUploadButton.Visibility = Visibility.Collapsed;
            ResultPageButton.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Event handler for the client page button click.
        /// Initializes a server communicator, starts communication, navigates to the ClientPage, and hides buttons.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ClientPageButtonClick(object sender, RoutedEventArgs e)
        {
            ICommunicator server = CommunicationFactory.GetServer();
            server.Start(null, null, ID.GetServerID(), "Content");
            Trace.WriteLine( "Server started: " + ID.GetServerID() );

            ServerPage serverPage = new (server, "TestServer");
            MainFrame.Navigate(serverPage);

            // TestClient by default
            serverPage.SetSessionID("TestClient");

            // Hide the buttons
            ResultPageButton.Visibility = Visibility.Collapsed;
            FileUploadButton.Visibility = Visibility.Collapsed;
        }
    }
}
