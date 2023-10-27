/******************************************************************************
 * Filename    = FileHandler.cs
 * 
 * Author      = Lekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Class that implements IFileHandler
 *****************************************************************************/

using Networking;
using Networking.Communicator;
using System.Diagnostics;

namespace Content
{
    /// <summary>
    /// Currently implemented as simply copying a file to the 
    /// context of the running application
    /// </summary>
    public class FileHandler : IFileHandler
    {
        public List<string> _filesList { get; set; }
        private ICommunicator _fileSender;
        private readonly Dictionary<string, string> _files;
        private readonly IFileEncoder _fileEncoder;
        /// <summary>
        /// saves files in //data/
        /// </summary>
        public FileHandler(ICommunicator fileSender) 
        {
            _files = new Dictionary<string, string>();
            _fileEncoder = new DLLEncoder();
            _filesList = new List<string>();
            _fileSender = fileSender;
        }

        /// <summary>
        /// Saves file in data location and calls analyzer query
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="sessionID"></param>
        public void Upload(string filepath, string sessionID)
        {
            // extract dll , and pass it to xml encoder use network functions to send
            // extracting paths of all dll files from the given directory
            string[] dllFiles = Directory.GetFiles(filepath, "*.dll", SearchOption.AllDirectories);
            string encoding = _fileEncoder.GetEncoded( dllFiles.ToList() );
            _filesList = dllFiles.ToList();
            Trace.Write( encoding );
            _fileSender.Send(encoding, "", "0.0.0.0");
        }

        /// <summary>
        /// Simply returns filepath in data location
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public string Download(string sessionID)
        {
            throw new NotImplementedException();
        }
    }
}
