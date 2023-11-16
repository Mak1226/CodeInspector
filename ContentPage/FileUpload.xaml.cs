/******************************************************************************
 * Filename    = FileUpload.xaml.cs
 * 
 * Author      = Sreelakshmi
 *
 * Product     = Analyzer
 * 
 * Project     = ContentPage
 *
 * Description = Page that lets user upload files for analysis
 *****************************************************************************/

using Content.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for FileUpload.xaml
    /// Represents the page for clients to see
    /// </summary>
    public partial class FileUpload : Page
    {
        private readonly ContentClientViewModel _client;

        /// <summary>
        /// Initialses the page
        /// </summary>
        /// <param name="client">The input ICommuncator that can send messages to server</param>
        /// <param name="sessionID">Some unique identifier for this user</param>
        public FileUpload(ContentClientViewModel client)
        {
            _client = client;
            InitializeComponent();
        }


        /// <summary>
        /// Handles a directory being provided to be uploaded
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            //Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //openFileDialog.Filter = "All files (*.*)|*.*|Dynamic Link Libraries (*.dll)|*.dll";
            //openFileDialog.Multiselect = false; // Set to true if you want to allow multiple file selection

            //System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.CheckFileExists = false;
            string defaultFilename = "This folder";
            ofd.FileName = defaultFilename;
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderPath = ofd.FileName;
                Trace.WriteLine(folderPath);
                // Pass folder path to Content Client
                if (Directory.Exists(folderPath.Substring(0, folderPath.Length - 12)))
                    _client.HandleUpload(folderPath.Substring(0, folderPath.Length - 12));
                else
                    _client.HandleUpload(folderPath);
            }
            //bool isFolderSelected = false;
            //string selectedPath = null;


        }




    }
}
