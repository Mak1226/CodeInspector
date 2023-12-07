/******************************************************************************
 * Filename    = SessionsPage.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View Logic of the Sessions Page.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cloud_UX;
using ServerlessFunc;

namespace ServerlessFuncUI
{
    /// <summary>
    /// Interaction logic for SessionsPage.xaml
    /// </summary>
    public partial class SessionsPage : Page
    {
        public string userName;
        public static int iterator = 0;
        public InsightPage1 insight_page_1;
        public InsightPage2 insight_page_2;
        public InsightPage3 insight_page_3;
        public InsightPage4 insight_page_4;
        public InsightPage5 insight_page_5;
        public InsightPage6 insight_page_6;
        public SessionsPage( string _HostName )
        {
            InitializeComponent();
            userName = _HostName;
            _viewModel = new SessionsViewModel( userName );
            DataContext = _viewModel;
            _viewModel.PropertyChanged += Listener;
            sessions = new List<SessionEntity> { };
            Trace.WriteLine( "[Cloud] Session View created Successfully" );
            insight_page_1 = new InsightPage1();
            insight_page_2 = new InsightPage2( userName );
            insight_page_3 = new InsightPage3( userName );
            insight_page_4 = new InsightPage4( userName );
            insight_page_5 = new InsightPage5( userName );
            insight_page_6 = new InsightPage6();


            Trace.WriteLine( "Bargraph page created" );
            SubmissionsPage.Content = insight_page_1;

        }

        private readonly SessionsViewModel _viewModel;

        public IReadOnlyList<SessionEntity>? sessions;

        public string UserName;

        /// <summary>
        /// OnPropertyChange handler to handle the change of sessions variable.
        /// </summary>
        private void Listener( object sender , PropertyChangedEventArgs e )
        {
            sessions = _viewModel.ReceivedSessions;

            if (sessions?.Count == 0)
            {
                Label label = new()
                {
                    Content = "No Sessions Conducted" ,
                    Foreground = new SolidColorBrush( Colors.White ) ,
                    HorizontalContentAlignment = HorizontalAlignment.Center ,
                    FontSize = 16
                };
                Stack.Children.Add( label );
                Trace.WriteLine( "[Cloud] No Sessions Conducted by " + UserName );

                return;
            }

            /*
             * Building the UI when there are list of sessions conducted. 
             * Adding Buttons for each session the host has conducted.
             */
            Trace.WriteLine( "[Cloud] Sessions data received to view" );
            for (int i = 0; i < sessions?.Count; i++)
            {
                Button newButton = new()
                {
                    Height = 30 ,
                    Margin = new Thickness( 0 , 5 , 0 , 5 ) ,
                    Name = "Button" + i.ToString() ,
                    Background = new SolidColorBrush( Colors.LightSkyBlue ) ,
                    Content = $"Session  {sessions[i].SessionId}"
                };
                newButton.Click += OnButtonClick;
                Stack.Children.Add( newButton );
                Trace.WriteLine( "[Cloud] Adding Button for the " + (i + 1) + "th Session" );
            }

        }

        /// <summary>
        /// Handler to the sessions button press
        /// </summary>
        private void OnButtonClick( object sender , RoutedEventArgs e )
        {
            Trace.WriteLine( "[Cloud] Session Button pressed" );
            Button caller = (Button)sender;
            int index = Convert.ToInt32( caller.Name.Split( 'n' )[1] );

            // Getting the Corresponding Submissions of the selected sessions and showing it in the place provided
            SubmissionsPage submissionsPage = new( sessions[index] );
            Trace.WriteLine( "[Cloud] SubmissionsPage created" );
            SubmissionsPage.Content = submissionsPage;
        }

        private void RefreshButtonClick( object sender , RoutedEventArgs e )
        {
            Trace.WriteLine( "[Cloud] Session Refresh Button pressed" );
            _viewModel.GetSessions( UserName );
            _viewModel.PropertyChanged += Listener;
        }

        private void RotateGraph( int add )
        {
            if (add == -1)
            {
                if (iterator == 0)
                {
                    iterator = 5;
                    SubmissionsPage.Content = insight_page_6;

                }
                else if (iterator == 1)
                {
                    iterator = 0;
                    SubmissionsPage.Content = insight_page_1;
                }
                else if (iterator == 2)
                {
                    iterator = 1;
                    SubmissionsPage.Content = insight_page_2;

                }
                else if (iterator == 3)
                {
                    iterator = 2;
                    SubmissionsPage.Content = insight_page_3;

                }
                else if (iterator == 4)
                {
                    iterator = 3;
                    SubmissionsPage.Content = insight_page_4;

                }
                else
                {
                    iterator = 4;
                    SubmissionsPage.Content = insight_page_5;
                }

            }
            else
            {
                if (iterator == 0)
                {
                    iterator = 1;
                    SubmissionsPage.Content = insight_page_2;

                }
                else if (iterator == 1)
                {
                    iterator = 2;
                    SubmissionsPage.Content = insight_page_3;
                }
                else if (iterator == 2)
                {
                    iterator = 3;
                    SubmissionsPage.Content = insight_page_4;

                }
                else if (iterator == 3)
                {
                    iterator = 4;
                    SubmissionsPage.Content = insight_page_5;

                }
                else if (iterator == 4)
                {
                    iterator = 5;
                    SubmissionsPage.Content = insight_page_6;

                }
                else
                {
                    iterator = 0;
                    SubmissionsPage.Content = insight_page_1;

                }

            }

        }

        private void LeftButtonClick( object sender , RoutedEventArgs e )
        {
            RotateGraph( -1 );
        }

        private void RightButtonClick( object sender , RoutedEventArgs e )
        {
            RotateGraph( 1 );
        }


    }
}
