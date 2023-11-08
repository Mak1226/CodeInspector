
using System.ComponentModel;

namespace Content.Server
{
    using AnalyzerResult = Tuple<Dictionary<string, string>, int>;

    public class ContentServerViewModel : INotifyPropertyChanged
    {
        private AnalyzerResult analyzerResult;
        private ContentServer contentServer;
        private List<Tuple<string, string, int>> dataList;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContentServerViewModel()
        {
            contentServer = new ContentServer();
            contentServer.AnalyzerResultChanged += (result) =>
            {
                analyzerResult = result;
                UpdateDataList(analyzerResult);
            };

        }

        public List<Tuple<string, string, int>> DataList
        {
            get 
            {
                return dataList; 
            }
            set { throw new NotImplementedException(); }
        }

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
