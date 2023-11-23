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
using ServerlessFunc;

namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for InsightPage5.xaml
    /// </summary>
    public partial class InsightPage5 : Page
    {
        private readonly InsightsApi _insightsApi;
        public static string InsightPath = "http://localhost:7074/api/insights";
        public string hostname;

        public InsightPage5(string user_name)
        {
            InitializeComponent();
            this.hostname = user_name;
            _insightsApi = new InsightsApi(InsightPath); 
        }

        private async void OnGetRunningAverageClick(object sender, RoutedEventArgs e)
        {
            try
            {

                List<double> averageList = await _insightsApi.RunningAverageAcrossSessoins(hostname);
                resultListBox.Text = "iohfew";
            }
              
            
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
