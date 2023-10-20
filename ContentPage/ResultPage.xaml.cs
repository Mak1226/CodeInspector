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
using System.Linq;
using System.Windows.Controls;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        public ResultPage()
        {
            InitializeComponent();

            var analysisResult = new Tuple<IDictionary<string , string> , int>(
                new Dictionary<string , string>
                {
                { "Key1", "Value1" },
                { "Key2", "Value2" },
                { "Key3", "Value3" }
                } ,
                42
            );

            var dataList = analysisResult.Item1.Select( kv => new
            {
                 kv.Key ,
                kv.Value ,
                analysisResult.Item2
            } ).ToList();

            var newdataList = analysisResult.Item1.Select( kv => new KeyValuePair<string , string>( kv.Key , kv.Value ) ).ToList();

            // Bind the list to the DataGrid
            dataGrid.ItemsSource = dataList;
        }
    }

}
