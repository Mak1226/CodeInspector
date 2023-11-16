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
        private Dictionary<string, List<AnalyzerResult>> analyzerResults;
        private ContentServer contentServer;
        private List<AnalyzerConfigOption> configOptionsList;
        //private Tuple<string, List<Tuple<string, int, string>>> dataList;


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        public ContentServerViewModel(ICommunicator server)
        {
            contentServer = new ContentServer(server, AnalyzerFactory.GetAnalyzer());
            contentServer.AnalyzerResultChanged += (result) =>
            {
                analyzerResults = result;
                //UpdateDataList(analyzerResults);
                OnPropertyChanged(nameof(analyzerResults));
                OnPropertyChanged(nameof(DataList));
            };

            // Populate ConfigOptionsList with data from AnalyzerFactory.GetAllConfigOptions
            configOptionsList = new List<AnalyzerConfigOption>();
            foreach (var option in AnalyzerFactory.GetAllConfigurationOptions())
            {
                configOptionsList.Add(new AnalyzerConfigOption
                {
                    AnalyzerId = option.Item1,
                    Description = option.Item2,
                    IsSelected = false // Set the default value for IsSelected as needed
                });
            }
            analyzerResults = contentServer.analyzerResult;
        }

        public void ConfigureAnalyzer(IDictionary<int, bool> teacherOptions)
        {
            // Call Analyzer.Configure
            contentServer.Configure(teacherOptions);
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
                if (analyzerResults == null)
                {
                    return new();
                }


                List<Tuple<string, List<Tuple<string, int, string>>>> outList = new();
                foreach (KeyValuePair<string, List<AnalyzerResult>> kvp in analyzerResults)
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

        public List<AnalyzerConfigOption> ConfigOptionsList
        {
            get { return configOptionsList; }
            set { configOptionsList = value; OnPropertyChanged(nameof(ConfigOptionsList)); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
