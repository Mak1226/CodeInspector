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
    public partial class ResultPage : Page
    {
        private ContentServerViewModel viewModel;

        /// <summary>
        /// Initializes content Server ViewModel
        /// </summary>
        /// <param name="server">Running server</param>
        public ResultPage(ICommunicator server)
        {
            InitializeComponent();
            viewModel = new ContentServerViewModel(server);
            DataContext = viewModel;

            //var analysisResult = new Tuple<IDictionary<string , string> , int>(
            //    new Dictionary<string , string>
            //    {
            //    { "Key1", "Value1" },
            //    { "Key2", "Value2" },
            //    { "Key3", "Value3" }
            //    } ,
            //    42
            //);

            //var dataList = analysisResult.Item1.Select( kv => new
            //{
            //     kv.Key ,
            //    kv.Value ,
            //    analysisResult.Item2
            //} ).ToList();

            //var newdataList = analysisResult.Item1.Select( kv => new KeyValuePair<string , string>( kv.Key , kv.Value ) ).ToList();

            //// Bind the list to the DataGrid
            //dataGrid.ItemsSource = dataList;
        }
        private void SelectCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var analyzerItem = checkBox?.DataContext as AnalyzerModel;

            if (analyzerItem != null && checkBox?.IsChecked == true)
            {
                YourFunction(analyzerItem.AnalyzerID, analyzerItem.IsSelected);
            }
        }
    }

}
