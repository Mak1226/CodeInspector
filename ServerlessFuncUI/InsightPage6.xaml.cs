/******************************************************************************
 * Filename    = InsightPage6.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View of the Insight Page 6, which includes Analysis of a session
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.AspNetCore.Mvc;
using ServerlessFunc;

namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for InsightPage6.xaml
    /// </summary>
    public partial class InsightPage6 : Page
    {
        private readonly InsightsApi _insightsApi;

        public InsightPage6()
        {
            InitializeComponent();

            // Create an instance of the InsightsApi class with the API endpoint
            _insightsApi = new InsightsApi("http://localhost:7074/api/insights");
        }

        /// <summary>
        /// Event handler for the "Get Students Without Analysis" button click.
        /// Retrieves and displays information related to student analysis based on the provided session ID.
        /// </summary>
        private async void OnGetStudentsWithoutAnalysis(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string sessionId = SessionIdTextBox.Text;

                // Retrieve lists and dictionaries containing various insights
                List<string> studentsWithoutAnalysis = await _insightsApi.UsersWithoutAnalysisGivenSession(sessionId);
                Dictionary<string, int> studentsscoreSession = await _insightsApi.GetStudentScoreGivenSession(sessionId);
                Dictionary<string, int> TestScore = await _insightsApi.GetTestScoreGivenSession(sessionId);
                List<string> BestWorst = await _insightsApi.GetBestWorstGivenSession(sessionId);
                Trace.WriteLine("all 4 lists retrieved from insight api");
                // Clear existing content in result text boxes
                resultListBox.Text = "";
                resultListBox1.Text = "";
                resultListBox2.Text = "";
                resultListBox3.Text = "";

                // Display the list of students without analysis in the first text box
                if (studentsWithoutAnalysis is not null)
                {
                    foreach (string cur in studentsWithoutAnalysis)
                    {
                        resultListBox.Text += $"{cur}\n";
                    }
                }
                else
                {
                    resultListBox.Text += $"null\n";
                }

                // Display the student scores in the second text box
                if (studentsscoreSession is not null)
                {
                    foreach (var cur in studentsscoreSession)
                    {
                        resultListBox1.Text += $"{cur.Key} {cur.Value}\n";
                    }
                }
                else
                {
                    resultListBox1.Text = "null";
                }

                // Display the test scores in the third text box
                if (TestScore is not null)
                {
                    foreach (var cur in TestScore)
                    {
                        resultListBox2.Text += $"{cur.Key} {cur.Value}\n";
                    }
                }
                else
                {
                    resultListBox2.Text += $"null\n";
                }

                // Display the best and worst marks in the fourth text box
                if (BestWorst is not null)
                {
                    resultListBox3.Text = $"Worst Marks: {BestWorst[0]}, Marks: {BestWorst[2]}\n" +
                        $"Best Marks: {BestWorst[1]}, Marks: {BestWorst[3]}";
                }
                else
                {
                    resultListBox3.Text = "null";
                }
                Trace.WriteLine("result displayed for insight page 6");
            }
            catch (Exception ex)
            {
                // Display an error message in case of an exception
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
