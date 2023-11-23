/******************************************************************************
 * Filename    = SessionsPage.xaml.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = Cloud_UX
 *
 * Description = View Model of the Submissions Page.
 *****************************************************************************/

using ServerlessFunc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;
using Cloud_UX;

namespace Cloud_UX
{
    public class SubmissionsViewModel :
        INotifyPropertyChanged // Notifies clients that a property value has changed.
    {
        /// <summary>
        /// Creates an instance of the Submissions ViewModel.
        /// Gets the details of the submissions of the session conducted by the user.
        /// Then dispatch the changes to the view.
        /// <param name="sessionId">Id of the session for which we want the submissions.</param>
        /// </summary>
        public SubmissionsViewModel(SessionEntity session)
        {
            _model = new SubmissionsModel();
            byte[] studentBytes = session.Students;
            string studentData = System.Text.Encoding.UTF8.GetString(studentBytes);

            // Split the string into a list of strings based on a delimiter (assuming, for example, that each student is separated by a comma)
            List<string> studentList = studentData.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();

            foreach (string name in studentList)
            {
                GetSubmissions(session.SessionId, name);
            }
            Trace.WriteLine("[Cloud] Submissions View Model Created");
        }

        /// <summary>
        /// Gets the details of the submissions of the session conducted by the user.
        /// Then dispatch the changes to the view.
        /// <param name="sessionId">Id of the session for which we want the submissions.</param>
        /// </summary>
        public async void GetSubmissions(string sessionId ,string studentName)
        {
            IReadOnlyList<SubmissionEntity> submissionsList = await _model.GetSubmissions(sessionId, studentName);
            Trace.WriteLine("[Cloud] Submission details recieved");
            _ = ApplicationMainThreadDispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action<IReadOnlyList<SubmissionEntity>>((submissionsList) =>
                        {
                            lock (this)
                            {
                                ReceivedSubmissions = submissionsList;

                                OnPropertyChanged("ReceivedSubmissions");
                            }
                        }),
                        submissionsList);
        }

        /// <summary>
        /// The received submissions.
        /// </summary>
        public IReadOnlyList<SubmissionEntity>? ReceivedSubmissions
        {
            get; private set;
        }

        /// <summary>
        /// Property changed event raised when a property is changed on a component.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Handles the property changed event raised on a component.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Gets the dispatcher to the main thread. In case it is not available
        /// (such as during unit testing) the dispatcher associated with the
        /// current thread is returned.
        /// </summary>
        private Dispatcher ApplicationMainThreadDispatcher =>
            (Application.Current?.Dispatcher != null) ?
                    Application.Current.Dispatcher :
                    Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Underlying data model.
        /// </summary>
        private readonly SubmissionsModel _model;

        /// <summary>
        /// To store which pdf to download.
        /// Call the corresponding function to download once the value is set.
        /// </summary>
        public int SubmissionToDownload
        {
            set => _model.DownloadPdf(value);
        }
    }
}
