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


namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for BarGraphPage
    /// </summary>
    public partial class InsightPage1 : Page
    {
        public static string InsightPath = "http://localhost:7074/api/insights";
        public static InsightsApi cur_Insight;

        public InsightPage1()
        {
            InitializeComponent();
            cur_Insight = new InsightsApi(InsightPath);

        }
        private async void OnSendButtonClick(object sender, RoutedEventArgs e)
        {
            // Get session IDs from textboxes
            string sessionId1 = sessionId1TextBox.Text;
            string sessionId2 = sessionId2TextBox.Text;

            try
            {
                // Call the CompareTwoSessions method
                List<Dictionary<string, int>> result = await cur_Insight.CompareTwoSessions(sessionId1, sessionId2);

                // Display the result in the ListBox
                DisplayResult(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayResult(List<Dictionary<string, int>> result)
        {
            // Clear the ListBox
            resultListBox.Items.Clear();

            // Display the result in the ListBox
            foreach (var dict in result)
            {
                string entry = "";
                foreach (var kvp in dict)
                {
                    entry += $"{kvp.Key}: {kvp.Value}, ";
                }
                resultListBox.Items.Add(entry.TrimEnd(',', ' '));
            }
        }


    }
}