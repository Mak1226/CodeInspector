/******************************************************************************
 * Filename    = ContentServerViewModel.cs
 * 
 * Author      = Jyothiradithya
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Network subscriber for handling client recieve
 *****************************************************************************/
using Analyzer;
using Content.Model;
using System.ComponentModel;

namespace Content.ViewModel
{
    /// <summary>
    /// Struct holding details about choosable analyzers
    /// </summary>
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

        /// <summary>
        /// Property change event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        /// <param name="contentServer">Content Server to be passed</param>
        public ContentServerViewModel(ContentServer contentServer)
        {
            _contentServer = contentServer;
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

        ///-----------Reactor functions-----------------------

        /// <summary>
        /// Configure Analyzer to the given analyzer
        /// </summary>
        /// <param name="teacherOptions">Dictionary of teacher options</param>
        public void ConfigureAnalyzer(IDictionary<int, bool> teacherOptions)
        {
            // Call Analyzer.Configure
            _contentServer.Configure(teacherOptions);
        }

        /// <summary>
        /// Set the sessionID of the server
        /// </summary>
        /// <param name="sessionID"></param>
        public void SetSessionID(string? sessionID)
        {
            _contentServer.SetSessionID(sessionID);
        }

        /// <summary>
        /// Load custom DLLs into the server
        /// </summary>
        /// <param name="filePaths">paths to the custom analyzer DLLs</param>
        public void LoadCustomDLLs(List<string> filePaths)
        {
            _contentServer.LoadCustomDLLs(filePaths);
            _uploadedFiles = filePaths;
            OnPropertyChanged(nameof(UploadedFiles));
        }
        
        /// <summary>
        /// Summarise data and send to cloud from server
        /// </summary>
        public void SendToCloud()
        {
            _contentServer.SendToCloud();
        }

        /// ------------MVVM bindings--------------

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

        /// <summary>
        /// Tab bindings
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
        /// Selectable list of configuration options
        /// </summary>
        public List<AnalyzerConfigOption> ConfigOptionsList
        {
            get { return _configOptionsList; }
            set { _configOptionsList = value; OnPropertyChanged(nameof(ConfigOptionsList)); }
        }

        /// <summary>
        /// Binding to show which all files are uploaded
        /// </summary>
        public string UploadedFiles => string.Join( "," , _uploadedFiles );

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
