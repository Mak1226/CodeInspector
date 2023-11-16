
using Analyzer;
using System.ComponentModel;

namespace Content.ViewModel
{
    public interface IContentViewModel
    {
        /// <summary>
        /// Event triggered when any property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Data for analysis results.
        /// If Server, depends on SessionID provided
        /// </summary>
        public List<Tuple<string, List<Tuple<string, int, string>>>> DataList { get; }

    }
}
