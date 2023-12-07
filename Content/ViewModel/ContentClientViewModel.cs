/******************************************************************************
 * Filename    = ContentClientViewModel.cs
 * 
 * Author      = Jyothiradithya
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Client side ViewModel
 *****************************************************************************/
using Analyzer;
using Content.Model;
using System.ComponentModel;

namespace Content.ViewModel
{
    /// <summary>
    /// View model for client, deriving off of IContentViewModel
    /// </summary>
    public class ContentClientViewModel : INotifyPropertyChanged, IContentViewModel
    {
        private readonly ContentClient _contentClient;
        private Dictionary<string, List<AnalyzerResult>> _analyzerResults;
        private Tuple<string, List<Tuple<string, int, string>>> _selectedItem;
        private ContentClient.StatusType _status;

        private bool _isDarkMode;

        /// <summary>
        /// Property change event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        public ContentClientViewModel(ContentClient contentClient)
        {
            _contentClient = contentClient;
            _contentClient.AnalyzerResultChanged += (result) =>
            {
                _analyzerResults = result;
                OnPropertyChanged(nameof(_analyzerResults));
                OnPropertyChanged(nameof(DataList));
            };
            _contentClient.ClientStatusChanged += ( status ) =>
            {
                _status = status;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(StatusColor));
            };
            _analyzerResults = _contentClient.analyzerResult;
        }

        /// <summary>
        /// Analysis result
        /// Currenly only shows the latest one
        /// 
        /// Dictionary keys are filenames. Entries are tuples of (Analyzer ID, Verdict, ErrorMessage)
        /// </summary>
        public List<Tuple<string, List<Tuple<string, int, string>>>> DataList
        {
            get
            {
                if (_analyzerResults == null)
                {
                    return new();
                }


                List<Tuple<string, List<Tuple<string, int, string>>>> outList = new();
                foreach (KeyValuePair<string, List<AnalyzerResult>> kvp in _analyzerResults)
                {
                    List<Tuple<string, int, string>> resultList = new();
                    foreach (AnalyzerResult result in kvp.Value)
                    {
                        resultList.Add(new(
                            result.AnalyserID,
                            result.Verdict,
                            result.ErrorMessage)
                            );
                    }
                    outList.Add(new(kvp.Key, resultList));
                }
                return outList;
            }
            set { throw new FieldAccessException(); }

        }

        /// <summary>
        /// ViewModel binding for tab navigation
        /// </summary>
        public Tuple<string, List<Tuple<string, int, string>>> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        /// <summary>
        /// Get the status of the client w.r.t the last data sent to server.
        /// </summary>
        public string Status
        {
            get
            {
                return _status switch
                {
                    ContentClient.StatusType.NONE => "Upload files to send to the instructor",
                    ContentClient.StatusType.WAITING => "Waiting for response from instructor",
                    ContentClient.StatusType.SUCCESS => "Files successfully analyzed",
                    ContentClient.StatusType.FAILURE => "Failed to analyze files. Please re-send",
                    _ => "Unknown error",
                };
            }
            set { throw new FieldAccessException();  }
        }

        /// <summary>
        /// Get the color of the status block
        /// </summary>
        public string StatusColor
        {
            get
            {
                return _status switch
                {
                    ContentClient.StatusType.NONE => "LightGray",
                    ContentClient.StatusType.WAITING => "AliceBlue",
                    ContentClient.StatusType.SUCCESS => "LimeGreen",
                    ContentClient.StatusType.FAILURE => "Red",
                    _ => "Unknown error",
                };
            }
            set { throw new FieldAccessException(); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Pass on to the client filepath to handle uploading at given path
        /// </summary>
        /// <param name="path">path of file/directory to upload</param>
        public void HandleUpload(string path)
        {
            _contentClient.HandleUpload(path);

        }

        /// <summary>
        /// Function to check if the app is in dark mode or light mode
        /// </summary>
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnPropertyChanged( nameof( IsDarkMode ) );
                }
            }
        }
    }
}
