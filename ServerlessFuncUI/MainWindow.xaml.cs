/******************************************************************************
 * Filename    = SessionsPage.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = ServerlessFuncUI
 *
 * Description = Defines the View of the Sessions Page.
 *****************************************************************************/

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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public SessionsPage(string id)
        {
            InitializeComponent();
            userName = id;
            viewModel = new SessionsViewModel(userName);
            this.DataContext = viewModel;
            viewModel.PropertyChanged += Listener;
            sessions = new List<SessionEntity> { };
            Trace.WriteLine("[Cloud] Session View created Successfully");
            insight_page_1 = new InsightPage1();
            insight_page_2 = new InsightPage2(userName);
            insight_page_3 = new InsightPage3(userName);
            insight_page_4 = new InsightPage4(userName);
            insight_page_5 = new InsightPage5(userName);
            insight_page_6 = new InsightPage6();


            Trace.WriteLine("Bargraph page created");
            SubmissionsPage.Content = insight_page_1;

        }
     
        private readonly SessionsViewModel viewModel;

        public IReadOnlyList<SessionEntity>? sessions;

        public string UserName;

        /// <summary>
        /// OnPropertyChange handler to handle the change of sessions variable.
        /// </summary>
        private void Listener(object sender, PropertyChangedEventArgs e)
        {
            sessions = viewModel.ReceivedSessions;

            
            if (sessions?.Count == 0)
            {
                Label label = new Label()
                {
                    Content = "No Sessions Conducted",
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontSize = 16
                };
                Stack.Children.Add(label);
                Trace.WriteLine("[Cloud] No Sessions Conducted by " + UserName);

                return;
            }

            /*
             * Building the UI when there are list of sessions conducted. 
             * Adding Buttons for each session the host has conducted.
             */
            Trace.WriteLine("[Cloud] Sessions data received to view");
            for (int i = 0; i < sessions?.Count; i++)
            {
                Button newButton = new Button();
                newButton.Height = 30;
                newButton.Margin = new Thickness(0, 5, 0, 5);
                newButton.Name = "Button" + i.ToString();
                newButton.Content = $"Session  {sessions[i].Id}";
                newButton.Click += OnButtonClick;
                Stack.Children.Add(newButton);
                Trace.WriteLine("[Cloud] Adding Button for the " + (i + 1) + "th Session");
            }

        }

        /// <summary>
        /// Handler to the sessions button press
        /// </summary>
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("[Cloud] Session Button pressed");
            Button caller = (Button)sender;
            int index = Convert.ToInt32(caller.Name.Split('n')[1]);

            // Getting the Corresponding Submissions of the selected sessions and showing it in the place provided
            SubmissionsPage submissionsPage = new SubmissionsPage(sessions[index].SessionId, UserName);
            Trace.WriteLine("[Cloud] SubmissionsPage created");
            SubmissionsPage.Content = submissionsPage;
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("[Cloud] Session Refresh Button pressed");
            viewModel.GetSessions(UserName);
            viewModel.PropertyChanged += Listener;
        }

        private void RotateGraph(int add)
        {
            if (add == -1)
            {
                if (iterator == 0)
                {
                    iterator = 5;
                    SubmissionsPage.Content = insight_page_6;

                }
                else if(iterator == 1)
                {
                    iterator = 0;
                    SubmissionsPage.Content = insight_page_1;
                }
                else if(iterator==2)
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
                else if(iterator==2)
                {
                    iterator = 3;
                    SubmissionsPage.Content = insight_page_4;

                }
                else if(iterator == 3)
                {
                    iterator = 4;
                    SubmissionsPage.Content = insight_page_5;

                }
                else if(iterator == 4)
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

        private void LeftButtonClick(object sender, RoutedEventArgs e)
        {
            RotateGraph(-1);
        }

        private void RightButtonClick(object sender, RoutedEventArgs e)
        {
            RotateGraph(1);
        }


    }
}
