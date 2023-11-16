using System.Windows.Controls;
using Analyzer;
using Content.Server;
using Networking.Communicator;

using System;
using System.Collections.Generic;
using System.Windows;


namespace ContentPage
{
    public partial class ClientPage : Page
    {
        private ContentServerViewModel viewModel;

        public ClientPage(ICommunicator server)
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

