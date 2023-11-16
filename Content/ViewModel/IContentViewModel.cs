
using Analyzer;
using System.ComponentModel;

namespace Content.ViewModel
{
    public interface IContentViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Tuple<string, List<Tuple<string, int, string>>>> DataList { get; }

    }
}
