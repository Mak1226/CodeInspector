using Analyzer;
using Content.Model;
using Networking.Communicator;
using System.ComponentModel;
using System.Diagnostics;

namespace Content.ViewModel
{
    public class AnalyzerConfigOption
    {
        public int AnalyzerId { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// Viewmodel for the Content Server model
    /// </summary>
    public class ContentServerViewModel : INotifyPropertyChanged, IContentViewModel
    {
        private readonly ContentServer _contentServer;
        private Dictionary<string, List<AnalyzerResult>> _analyzerResults;
        private List<AnalyzerConfigOption> _configOptionsList;
        private Tuple<string, List<Tuple<string, int, string>>> _selectedItem;
        private List<string> _uploadedFiles = new();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        public ContentServerViewModel(ICommunicator server, string sessionID)
        {
            _contentServer = new ContentServer(server, AnalyzerFactory.GetAnalyzer(), sessionID);
            _contentServer.AnalyzerResultChanged += (result) =>
            {
                _analyzerResults = result;
                //UpdateDataList(analyzerResults);
                OnPropertyChanged(nameof(_analyzerResults));
                OnPropertyChanged(nameof(DataList));
            };

            // Populate ConfigOptionsList with data from AnalyzerFactory.GetAllConfigOptions
            _configOptionsList = new List<AnalyzerConfigOption>();
            foreach (Tuple<int , string> option in AnalyzerFactory.GetAllConfigurationOptions())
            {
                _configOptionsList.Add(new AnalyzerConfigOption
                {
                    AnalyzerId = option.Item1,
                    Description = option.Item2,
                    IsSelected = false // Set the default value for IsSelected as needed
                });
            }
            _analyzerResults = _contentServer.analyzerResult;
        }

        public void ConfigureAnalyzer(IDictionary<int, bool> teacherOptions)
        {
            // Call Analyzer.Configure
            _contentServer.Configure(teacherOptions);
        }

        public void SetSessionID(string? sessionID)
        {
            _contentServer.SetSessionID(sessionID);
        }

        public void LoadCustomDLLs(List<string> filePaths)
        {
            _contentServer.LoadCustomDLLs(filePaths);
            _uploadedFiles = filePaths;
            OnPropertyChanged(nameof(UploadedFiles));
        }

        public void SendToCloud()
        {
            _contentServer.SendToCloud();
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

        }

        public Tuple<string, List<Tuple<string, int, string>>> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public List<AnalyzerConfigOption> ConfigOptionsList
        {
            get { return _configOptionsList; }
            set { _configOptionsList = value; OnPropertyChanged(nameof(ConfigOptionsList)); }
        }


        public string UploadedFiles => string.Join( "," , _uploadedFiles );

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
