using Analyzer;
using Content.Model;
using System.ComponentModel;

namespace Content.ViewModel
{
    public class ContentClientViewModel : INotifyPropertyChanged, IContentViewModel
    {
        private readonly ContentClient _contentClient;
        private Dictionary<string, List<AnalyzerResult>> _analyzerResults;
        private Tuple<string, List<Tuple<string, int, string>>> _selectedItem;

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
    }
}
