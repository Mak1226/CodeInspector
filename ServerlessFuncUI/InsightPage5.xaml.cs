/*
  *Filename = InsightPage5.xaml.cs
  *
  *Author = Sidharth Chadha
  *
  * Project = ServerlessFuncUI
  *
  *Description = Defines the Bar Graph View of Test Averages ( Insight Page 5)
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
    /// Interaction logic for InsightPage5.xaml
    /// </summary>
    public partial class InsightPage5 : Page, INotifyPropertyChanged
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
            Trace.WriteLine("bar graph updated for insight page 5");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public InsightPage5(string user_name)
        {
            InitializeComponent();
            hostname = user_name;
            _insightsApi = new InsightsApi(InsightPath);
            _meanValues = new ChartValues<ObservableValue> { new ObservableValue(0) };
            DataContext = this;
            build();
        }
        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void build()
        {
            try
            {

                List<double> averageList = await _insightsApi.RunningAverageAcrossSessoins(hostname);
                _meanValues.Clear();
                Trace.WriteLine("average list updated (retrieved)");
                // Create a ColumnSeries and add the average values to it
                if (averageList is not null)
                {
                    foreach (double average in averageList)
                    {
                        _meanValues.Add( new ObservableValue( average ) );
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
