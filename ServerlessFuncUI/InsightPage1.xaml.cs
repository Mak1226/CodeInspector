/******************************************************************************
 * Filename    = InsightPage1.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View logic of Insight Page 1
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using ServerlessFunc;


namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for BarGraphPage
    /// </summary>
    public partial class InsightPage1 : Page
    {
        //public static string InsightPath = "http://localhost:7074/api/insights";
        public static string InsightPath = "https://serverlessfunc20231121082343.azurewebsites.net/api/insights";
     
        public static InsightsApi cur_Insight;

        public InsightPage1()
        {
            InitializeComponent();
            cur_Insight = new InsightsApi( InsightPath );
            Trace.WriteLine( " insight page 1 created" );
        }
        private async void OnSendButtonClick( object sender , RoutedEventArgs e )
        {
            // Get session IDs from textboxes
            string sessionId1 = sessionId1TextBox.Text;
            string sessionId2 = sessionId2TextBox.Text;

            try
            {
                // Call the CompareTwoSessions method
                List<Dictionary<string , int>> result = await cur_Insight.CompareTwoSessions( sessionId1 , sessionId2 );

                // Display the result in the ListBox
                if (result is not null)
                {
                    DisplayResult( result );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( $"Error: {ex.Message}" , "Error" , MessageBoxButton.OK , MessageBoxImage.Error );
            }
        }

        private void DisplayResult( List<Dictionary<string , int>> result )
        {


            // Clear existing series in both graphs
            graph1.Series.Clear();
            graph2.Series.Clear();

            foreach (Dictionary<string , int> dict in result)
            {
                IOrderedEnumerable<string> sortedKeys = dict.Keys.OrderBy( key => key );
                var series = new LineSeries()
                {
                    Title = "Graph " + (result.IndexOf( dict ) + 1) ,
                    Values = new ChartValues<int>() ,
                };

                // Populate the series with data from the dictionary
                foreach (string key in sortedKeys)
                {
                    series.Values.Add( dict[key] );
                }

                // Add the series to the appropriate graph
                if (result.IndexOf( dict ) == 0)
                {
                    graph1.Series.Add( series );
                }
                else
                {
                    graph2.Series.Add( series );
                }
            }

            string[] xLabels = result.FirstOrDefault()?.Keys.ToArray();

            if (xLabels != null)
            {
                // Set X-axis labels for both graphs
                graph1.AxisX.Clear();
                graph1.AxisX.Add( new Axis { Labels = xLabels } );

                graph2.AxisX.Clear();
                graph2.AxisX.Add( new Axis { Labels = xLabels } );
            }




            Trace.WriteLine( "result displayed for insight page 1" );
        }




    }
}
