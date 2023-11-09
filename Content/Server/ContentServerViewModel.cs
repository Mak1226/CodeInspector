using Analyzer;
using Networking.Communicator;
using System.ComponentModel;

namespace Content.Server
{

    /// <summary>
    /// Viewmodel for the Content Server model
    /// </summary>
    public class ContentServerViewModel : INotifyPropertyChanged
    {
        private List<AnalyzerResult> analyzerResults;
        private ContentServer contentServer;
        private List<Tuple<string, int, string>> dataList;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        public ContentServerViewModel(ICommunicator server)
        {
            IAnalyzer analyzer = AnalyzerFactory.GetAnalyzer();

            contentServer = new ContentServer(server, analyzer);
            contentServer.AnalyzerResultChanged += (result) =>
            {
                analyzerResults = result;
                UpdateDataList(analyzerResults);
            };

        }

        /// <summary>
        /// Analysis result
        /// Currenly only shows the latest one
        /// </summary>
        public List<Tuple<string, int, string>> DataList
        {
            get 
            {
                return dataList; 
            }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Update data list whenever analyzer result in content server is updated
        /// </summary>
        /// <param name="analyzerResults">Analyzer Result from content server</param>
        public void UpdateDataList(List<AnalyzerResult> analyzerResults)
        {
            dataList = new();
            foreach (AnalyzerResult result in analyzerResults)
            {
                dataList.Add(new Tuple<string, int, string>(result.AnalyserID, result.Verdict, result.ErrorMessage));
            }

            OnPropertyChanged(nameof(dataList));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
