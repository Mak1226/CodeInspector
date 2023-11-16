/******************************************************************************
 * Filename    = ResultPage.xaml.cs
 * 
 * Author      = Sreelekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = ContentPage
 *
 * Description = Page that visualises result of an analysis
 *****************************************************************************/

using System.Windows.Controls;
using Content.ViewModel;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        private readonly IContentViewModel _viewModel;
        private string _sessionID;

        /// <summary>
        /// Initializes content Server ViewModel
        /// </summary>
        /// <param name="server">Running server</param>
        public ResultPage(IContentViewModel viewModel, string sessionID)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _sessionID = sessionID;
        }
       

        
    }

}
