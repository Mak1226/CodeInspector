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
using Content.ViewModel;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        private readonly ContentServerViewModel _viewModel;
        private readonly IDictionary<int, bool> _accumulatedOptions = new Dictionary<int, bool>();

        /// <summary>
        /// Initializes content Server ViewModel
        /// </summary>
        /// <param name="server">Running server</param>
        public ConfigurationPage(ContentServerViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }
        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            // Handle the CheckBox checked event if needed
            // For example, you might call a method on viewModel
            var checkBox = sender as CheckBox;

            if (checkBox?.DataContext is AnalyzerConfigOption analyzerItem && checkBox?.IsChecked == true)
            {
                // Assuming Configure is a method on viewModel
                int analyzerId = Convert.ToInt32( analyzerItem.AnalyzerId );
                _accumulatedOptions[analyzerId] = true;
                _viewModel.ConfigureAnalyzer( _accumulatedOptions );
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

            if (checkBox?.DataContext is AnalyzerConfigOption analyzerItem)
            {
                int analyzerId = Convert.ToInt32( analyzerItem.AnalyzerId );
                _accumulatedOptions.Remove( analyzerId );

                _viewModel.ConfigureAnalyzer( _accumulatedOptions );
            }
        }





    }

}

