/******************************************************************************
 * Filename    = ClientPage.xaml.cs
 * 
 * Author      = Sreelakshmi
 *
 * Product     = Analyser
 * 
 * Project     = ContentPage
 *
 * Description = This file contains the code-behind for the ClientPage.xaml.
 *              
 *****************************************************************************/
using Content.ViewModel;
using Content.Model;
using Networking.Communicator;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows; 


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
            Trace.WriteLine( "Initializing ClientPage" );
            InitializeComponent();
            _viewModel = new ContentClientViewModel(new ContentClient(client, sessionID));
            DataContext = _viewModel;
           
            Trace.WriteLine( "Navigating to FileUpload and ResultPage" );
            UploadFrame.Navigate(new FileUpload(_viewModel));
            ResultFrame.Navigate(new ResultPage(_viewModel));
        }

        /// <summary>
        /// Function to toggle dark mode in client
        /// </summary>
        /// <param name="darkMode"></param>
        public void SetDarkMode( bool darkMode )
        {
            _viewModel.IsDarkMode = darkMode;
        }
    }
}
