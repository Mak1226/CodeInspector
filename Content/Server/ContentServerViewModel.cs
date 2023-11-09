using Analyzer;
using Networking.Communicator;
using System.ComponentModel;

namespace Content.Server
{
    using AnalyzerResult = Tuple<Dictionary<string, string>, int>;

    /// <summary>
    /// Viewmodel for the Content Server model
    /// </summary>
    public class ContentServerViewModel : INotifyPropertyChanged
    {
        private AnalyzerResult analyzerResult;
        private ContentServer contentServer;
        private List<Tuple<string, string, int>> dataList;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes Content Server and provides it server and analyzer
        /// </summary>
        public ContentServerViewModel(ICommunicator server)
        {
            IAnalyzer analyzer = AnalyzerFactory.GetAnalyser();

            contentServer = new ContentServer(server, analyzer);
            contentServer.AnalyzerResultChanged += (result) =>
            {
                analyzerResult = result;
                UpdateDataList(analyzerResult);
            };

        }

        /// <summary>
        /// Analysis result
        /// Currenly only shows the latest one
        /// </summary>
        public List<Tuple<string, string, int>> DataList
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
        /// <param name="analyzerResult">Analyzer Result from content server</param>
        public void UpdateDataList(AnalyzerResult analyzerResult)
        {
            dataList = new();
            foreach (var kvp in analyzerResult.Item1)
            {
                dataList.Add(new Tuple<string, string, int>(kvp.Key, kvp.Value, analyzerResult.Item2));
            }

            OnPropertyChanged(nameof(dataList));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
