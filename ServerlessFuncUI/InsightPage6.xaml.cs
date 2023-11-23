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
            _insightsApi = new InsightsApi("http://localhost:7074/api/insights");
        }

        private async void OnGetStudentsWithoutAnalysis(object sender, RoutedEventArgs e)
        {
            try
            {
                // Assuming you have a session ID, replace "YOUR_SESSION_ID" with the actual session ID
                string sessionId = SessionIdTextBox.Text ;

                List<string> studentsWithoutAnalysis = await _insightsApi.UsersWithoutAnalysisGivenSession(sessionId);
                Dictionary<string, int> studentsscoreSession = await _insightsApi.GetStudentScoreGivenSession(sessionId);
                Dictionary<string, int> TestScore = await _insightsApi.GetTestScoreGivenSession(sessionId);
                List<string> BestWorst = await _insightsApi.GetBestWorstGivenSession(sessionId);


                // Display the list of students in the ListBox

                foreach (string cur in BestWorst)
                {
                    resultListBox.Text += $"{cur}\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
