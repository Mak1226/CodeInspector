/*
  *Filename = InsightPage4.xaml.cs
  *
  *Author = Sidharth Chadha
  *
  * Project = ServerlessFuncUI
  *
  *Description = Defines the view logic of Insight Page 4 ( bar graph for running averages)
  **************************************************************************** */
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
using System.ComponentModel;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts;
using System.Diagnostics;

namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for InsightPage4.xaml
    /// </summary>
    public sealed partial class InsightPage4 : Page, INotifyPropertyChanged
    {
        private readonly InsightsApi _insightsApi;
        public static string InsightPath = "http://localhost:7074/api/insights";
        public string hostname;
        private ChartValues<ObservableValue> _meanValues;

        public event PropertyChangedEventHandler PropertyChanged;
        public ChartValues<ObservableValue> MeanValues
        {
            get => _meanValues;
            set
            {
                _meanValues = value;
                OnPropertyChanged(nameof(MeanValues));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            Trace.WriteLine("bar graph updated for insight page 4");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public InsightPage4(string host_name)
        {
            InitializeComponent();
            hostname = host_name;

            // Initialize InsightsApi with the appropriate insightsRoute
            _insightsApi = new InsightsApi(InsightPath);
            _meanValues = new ChartValues<ObservableValue> { new ObservableValue(0) };
            DataContext = this;
        }
        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void OnGetStudentAverageClick(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string studentName = StudentNameTextBox.Text;

                // Call the RunningAverageOnGivenStudent method from InsightsApi
                List<double> averageList = await _insightsApi.RunningAverageOnGivenStudent(hostname, studentName);
                Trace.WriteLine("averages retrieved for student ");
                _meanValues.Clear();
                // Create a ColumnSeries and add the average values to it
                if(averageList != null) 
                { 
                    foreach (double average in averageList)
                    {
                        _meanValues.Add(new ObservableValue(average));
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
        }
    }

}
