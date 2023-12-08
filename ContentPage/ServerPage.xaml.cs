/******************************************************************************
 * Filename    = ServerPage.xaml.cs
 * 
 * Author      = Sreelakshmi
 *
 * Product     = Analyser
 * 
 * Project     = ContentPage
 *
 * Description = This file contains the code-behind for the ServerPage.xaml.
 *             
 *****************************************************************************/

using System.Windows.Controls;
using Content.ViewModel;
using Networking.Communicator;
using System.Windows.Forms;
using System.Collections.Generic;
using Analyzer;
using Content.Model;
using System.Diagnostics;
using System.IO;

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
        /// <param name="sessionID">Unique ID of server</param>
        public ServerPage(ICommunicator server, string sessionID)
        {
            Trace.WriteLine( "Initializing ServerPage" );

            InitializeComponent();
            _viewModel = new ContentServerViewModel(
                new ContentServer( server, AnalyzerFactory.GetAnalyzer(), sessionID )
                );
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
            Trace.WriteLine( $"Setting session ID: {sessionID}" );

            _viewModel.SetSessionID(sessionID);
        }

        /// <summary>
        /// Function to toggle dark mode in server
        /// </summary>
        /// <param name="darkMode"></param>
        public void SetDarkMode(bool darkMode)
        {
            _viewModel.IsDarkMode = darkMode;
        }

        /// <summary>
        /// Loads the ResultPage into the ResultFrame.
        /// </summary>
        private void LoadResultPage()
        {
            Trace.WriteLine( "Loading ResultPage" );

            ResultPage resultPage = new (_viewModel);
            ResultFrame.NavigationService.Navigate(resultPage);
            
        }
        /// <summary>
        /// Loads the ConfigurationPage into the ConfigFrame.
        /// </summary>
        private void LoadConfigurationPage()
        {
            Trace.WriteLine( "Loading ConfigurationPage" );

            ConfigurationPage configPage = new (_viewModel);
            ConfigFrame.NavigationService.Navigate(configPage);
            
        }

        /// <summary>
        /// Event handler for the AnalyzerUploadButton click. Allows uploading DLL files for analysis.
        /// </summary>
        private void AnalyzerUploadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Trace.WriteLine( "AnalyzerUploadButton Click" );

            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true , // Allow multiple file selection
                Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*" // Filter for DLL files
            };

            // Show the dialog and get the result
            DialogResult result = openFileDialog.ShowDialog();

            // Process the selected files
            if (result == DialogResult.OK)
            {
                List<string> filePaths = new (openFileDialog.FileNames);
                _viewModel.LoadCustomDLLs(filePaths);

                Trace.WriteLine( "Custom DLLs loaded" );

            }
        }

        /// <summary>
        /// Event handler for the SendToCloudButton click. Initiates sending data to the cloud.
        /// </summary>
        private void SendToCloudButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Trace.WriteLine( "SendToCloudButton Click" );

            _viewModel.SendToCloud();
        }

        private void ShowImageButton_Click( object sender , System.Windows.RoutedEventArgs e )
        {
            string imagePath = _viewModel.GetImagePath();
            if (File.Exists( imagePath ))
            {
                try
                {
                    Process.Start( imagePath ); // Opens the image with the default application
                }
                catch
                {
                    MessageBox.Show( $"Couldn't open image" , "Error" , MessageBoxButtons.OK );
                }
            }
            else
            {
                // Display a message indicating that the image is not present
                MessageBox.Show( $"Image not found at the specified path {imagePath}" , "Image Not Found" , MessageBoxButtons.OK);
            }
        }
    }
}

