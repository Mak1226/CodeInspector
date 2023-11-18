/******************************************************************************
 * Filename    = BarGraphPage.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View of the Bar Graph Page.
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
using System.Diagnostics.Contracts;

namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for BarGraphPage
    /// </summary>
    public sealed partial class InsightPage3 : Page
    {
        private readonly InsightsApi _insightsApi;
        public string InsightPath = "http://localhost:7074/api/insights";
        public string hostname;
        public InsightPage3(string host_name)
        {
            this.InitializeComponent();
            this.hostname = hostname;
            _insightsApi = new InsightsApi(InsightPath);
        }

        private async void GetRunningAverage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Replace "your_hostname" and "your_test_name" with the actual values
               
                string testName = TestNameTextBox.Text;

                // Call the RunningAverageOnGivenTest method from InsightsApi
                List<double> averageList = await _insightsApi.RunningAverageOnGivenTest(hostname, testName);

                // Handle the results, update the TextBlock with the averages
               // resultListBlock.Text = "Running Averages:\n";
                foreach (double average in averageList)
                {
                    resultListBox.Text += $"{average}\n";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, JSON parsing errors)
                // You might want to display an error message to the user
                // MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                resultListBox.Text = $"Error: {ex.Message}";
            }
        }
    }
}