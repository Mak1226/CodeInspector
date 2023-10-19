using Content;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ContentPage
{
    /// <summary>
    /// Interaction logic for FileUpload.xaml
    /// </summary>
    public partial class FileUpload : Page
    {
        public FileUpload()
        {
            InitializeComponent();
        }

        //private void UploadButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
        //    bool? response = openFileDialog.ShowDialog();
        //    if (response == true)
        //    {
        //        IFileHandler upload_file = new FileHandler();
        //        string filepath = openFileDialog.FileName;
        //        Trace.WriteLine(filepath);
        //        //upload_file.Upload(filepath,"5");
        //    }

        //}




        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string folderPath = folderDialog.SelectedPath;
                    Trace.WriteLine(folderPath);

                    // Now, you have the folderPath, and you can use it as needed.
                    // For example, you can pass it to your file handler.
                    IFileHandler upload_file = new FileHandler();
                    //upload_file.Upload(folderPath, "5");
                }
            }
        }



    }
}
