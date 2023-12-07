
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
        public List<Tuple<string , List<Tuple<string , int , string>>>> DataList { get; }

        /// <summary>
        /// Two way binding for tab selection in view
        /// </summary>
        public Tuple<string , List<Tuple<string , int , string>>> SelectedItem { get; set; }

        /// <summary>
        /// Dark mode for all Content Pages
        /// </summary>
        public bool IsDarkMode { get; set; }
    }
}
