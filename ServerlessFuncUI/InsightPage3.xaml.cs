/******************************************************************************
 * Filename    = InsightPage3.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View logic of the Bar Graph Page for running averages of session
 *****************************************************************************/
using LiveCharts.Defaults;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ServerlessFunc;
using LiveCharts.Wpf;
using System.Diagnostics;

namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for BarGraphPage
    /// </summary>
    public sealed partial class InsightPage3 : Page, INotifyPropertyChanged
    {
        private readonly InsightsApi _insightsApi;
        public string InsightPath = "http://localhost:7074/api/insights";
        public string hostname;
        private ChartValues<ObservableValue> meanValues;

        public event PropertyChangedEventHandler PropertyChanged;

        public ChartValues<ObservableValue> MeanValues
        {
            get => meanValues;
            set
            {
                meanValues = value;
                OnPropertyChanged(nameof(MeanValues));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public InsightPage3(string host_name)
        {
            InitializeComponent();
            hostname = host_name;

            _insightsApi = new InsightsApi(InsightPath);
            meanValues = new ChartValues<ObservableValue> { new ObservableValue(0) };
            DataContext = this;
        }
        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void GetRunningAverage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string testName = TestNameTextBox.Text;
                List<double> averageList = await _insightsApi.RunningAverageOnGivenTest(hostname, testName);
                Trace.WriteLine("retrieved runinng averages and diplayed the changes in bar graph");
                meanValues.Clear();
                // Create a ColumnSeries and add the average values to it
                foreach (double average in averageList)
                {
                    meanValues.Add(new ObservableValue(average));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}



