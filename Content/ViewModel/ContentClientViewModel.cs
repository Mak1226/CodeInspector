using Analyzer;
using Content.Model;
using Networking.Communicator;
using System.ComponentModel;

namespace Content.ViewModel
{
    public class ContentClientViewModel : INotifyPropertyChanged, IContentViewModel
    {
        private Dictionary<string, List<AnalyzerResult>> analyzerResults;
        private ContentClient contentClient;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        public ContentClientViewModel(ICommunicator client, string sessionID)
        {
            contentClient = new ContentClient(client, sessionID);
            contentClient.AnalyzerResultChanged += (result) =>
            {
                analyzerResults = result;
                OnPropertyChanged(nameof(analyzerResults));
                OnPropertyChanged(nameof(DataList));
            };
            analyzerResults = contentClient.analyzerResult;
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
            set { throw new FieldAccessException(); }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void HandleUpload(string path)
        {
            contentClient.HandleUpload(path);
        }
    }
}
