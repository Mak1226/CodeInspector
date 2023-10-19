using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
