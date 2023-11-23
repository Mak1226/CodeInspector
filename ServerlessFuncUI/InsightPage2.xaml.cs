/******************************************************************************
 * Filename    = InsightPage2.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View logic of Insight Page 2
 *****************************************************************************/
using LiveCharts.Defaults;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Cloud_UX;
using System.Diagnostics;
using System.Collections;


namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for BarGraphPage
    /// </summary>
    public partial class InsightPage2 : Page
    {
        
        private readonly InsightsApi _insightsApi;
        public static string InsightPath = "http://localhost:7074/api/insights";
        public string hostname;
        public InsightPage2(string hostname)
        {
            InitializeComponent();
            _insightsApi = new InsightsApi(InsightPath);
            this.hostname = hostname;
            Trace.WriteLine("insight page 2 created");
        }

        private async void OnGetFailedStudentsClick(object sender, RoutedEventArgs e)
        {
            // Assuming you have the hostname and testName from some input fields.
            
            string testName = TestNameTextBox.Text;

            try
            {
                List<string> failedStudents = await _insightsApi.GetFailedStudentsGivenTest(hostname, testName);
                Trace.WriteLine("retrieved failed students");
                // Assuming you want to display the results in a ListView.
                resultListBox.ItemsSource = failedStudents;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
