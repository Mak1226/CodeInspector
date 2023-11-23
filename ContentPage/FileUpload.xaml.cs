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
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for FileUpload.xaml
    /// Represents a simple file upload button
    /// </summary>
    public partial class FileUpload : Page
    {
        private readonly ContentClientViewModel _client;

        /// <summary>
        /// Page for uploading files
        /// </summary>
        /// <param name="client">client view model</param>
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
            try
            {
                
                OpenFileDialog ofd = new()
                {
                    CheckFileExists = false
                };
                string defaultFilename = "This folder";
                ofd.FileName = defaultFilename;
                DialogResult result = ofd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string folderPath = ofd.FileName;
                    Trace.WriteLine(folderPath);
                    // Pass folder path to Content Client
                    if (Directory.Exists(folderPath.Substring(0, folderPath.Length - 12)))
                    {
                        _client.HandleUpload(folderPath.Substring(0, folderPath.Length - 12));
                    }
                    else
                    {
                        _client.HandleUpload(folderPath);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

        }

    }
}
