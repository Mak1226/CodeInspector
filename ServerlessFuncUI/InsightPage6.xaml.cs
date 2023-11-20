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
    /// Interaction logic for InsightPage6.xaml
    /// </summary>
    public partial class InsightPage6 : Page
    {
        private readonly InsightsApi _insightsApi;

        public InsightPage6()
        {
            InitializeComponent();
            _insightsApi = new InsightsApi("path");
        }

        private async void OnGetStudentsWithoutAnalysis(object sender, RoutedEventArgs e)
        {
            try
            {
                // Assuming you have a session ID, replace "YOUR_SESSION_ID" with the actual session ID
                string sessionId = "YOUR_SESSION_ID";

                List<string> studentsWithoutAnalysis = await _insightsApi.UsersWithoutAnalysisGivenSession(sessionId);

                // Display the list of students in the ListBox
                studentsListBox.ItemsSource = studentsWithoutAnalysis;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
