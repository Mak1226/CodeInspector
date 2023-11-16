/******************************************************************************
 * Filename    = ResultPage.xaml.cs
 * 
 * Author      = Sreelekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = ContentPage
 *
 * Description = Page that visualises result of an analysis
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Analyzer;
using Content.Server;
using Networking.Communicator;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        private ContentServerViewModel viewModel;
        private IDictionary<int, bool> accumulatedOptions = new Dictionary<int, bool>();

        /// <summary>
        /// Initializes content Server ViewModel
        /// </summary>
        /// <param name="server">Running server</param>
        public ConfigurationPage(ContentServerViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
        }
        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            // Handle the CheckBox checked event if needed
            // For example, you might call a method on viewModel
            var checkBox = sender as CheckBox;
            var analyzerItem = checkBox?.DataContext as AnalyzerConfigOption;

            if (analyzerItem != null && checkBox?.IsChecked == true)
            {
                // Assuming Configure is a method on viewModel
                int analyzerId = Convert.ToInt32(analyzerItem.AnalyzerId);
                accumulatedOptions[analyzerId] = true;
                viewModel.ConfigureAnalyzer(accumulatedOptions, true);
                //        viewModel.ConfigureAnalyzer(new Dictionary<int, bool>
                //{
                //    { Convert.ToInt32(analyzerItem.AnalyzerId), true }
                //}, true);
                //    }
            }
        }
        private void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var analyzerItem = checkBox?.DataContext as AnalyzerConfigOption;

            if (analyzerItem != null)
            {
                int analyzerId = Convert.ToInt32(analyzerItem.AnalyzerId);
                accumulatedOptions.Remove(analyzerId);

                viewModel.ConfigureAnalyzer(accumulatedOptions, true);
            }
        }





    }

}

