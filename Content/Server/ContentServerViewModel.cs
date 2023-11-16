using Analyzer;
using Networking.Communicator;
using System.Collections.Generic;
using System.ComponentModel;

namespace Content.Server
{

    /// <summary>
    /// Viewmodel for the Content Server model
    /// </summary>
    public class ContentServerViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, List<AnalyzerResult>> analyzerResults;
        private ContentServer contentServer;
        //private Tuple<string, List<Tuple<string, int, string>>> dataList;

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
                //UpdateDataList(analyzerResults);
                OnPropertyChanged(nameof(analyzerResults));
                OnPropertyChanged(nameof(DataList));
            };

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
            set { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Update data list whenever analyzer result in content server is updated
        /// </summary>
        /// <param name="analyzerResults">Analyzer Result from content server</param>
        //public void UpdateDataList(Dictionary<string, List<AnalyzerResult>> analyzerResults)
        //{
        //    dataList = new();
        //    foreach (KeyValuePair<string, List<AnalyzerResult>> kvp in analyzerResults)
        //    {
        //        foreach (AnalyzerResult result in kvp.Value)
        //        {
        //            dataList[kvp.Key].Add(
        //                new Tuple<string, int, string>(
        //                    result.AnalyserID,
        //                    result.Verdict,
        //                    result.ErrorMessage
        //                    )
        //                );
        //        }
        //    }

        //    OnPropertyChanged(nameof(dataList));
        //}

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
