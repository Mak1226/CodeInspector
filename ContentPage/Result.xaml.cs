using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;


namespace ContentPage
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Page
    {
        public Result()
        {
            InitializeComponent();
          
            var analysisResult = new Tuple<IDictionary<string, string>, int>(
                new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" },
                    { "Key3", "Value3" }
                },
                42
            );

            // Populate the DataGrid
            dataGrid.ItemsSource = analysisResult.Item1; // Bind the dictionary to the DataGrid
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Value", Binding = new System.Windows.Data.Binding("Value") }); // Add a column for the dictionary values

            // Add a column for the int value
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Integer", Binding = new System.Windows.Data.Binding("Item2") });
        }
    }
}
