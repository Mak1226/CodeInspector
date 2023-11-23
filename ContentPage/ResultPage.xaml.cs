/******************************************************************************
 * Filename    = ResultPage.xaml.cs
 * 
 * Author      = Sreelakshmi
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

        /// <summary>
        /// Initializes content Server ViewModel
        /// </summary>
        /// <param name="viewModel">Content view model</param>
        public ResultPage(IContentViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }

}
