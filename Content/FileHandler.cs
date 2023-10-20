/******************************************************************************
* Author      = Susan
*
* Product     = Analyzer
* 
* Project     = Content
*
* Description = Class that implements IFileHandler
*****************************************************************************/

using System.Diagnostics;

namespace Content
{
    /// <summary>
    /// Currently implemented as simply copying a file to the 
    /// context of the running application
    /// </summary>
    public class FileHandler : IFileHandler
    {
        private readonly Dictionary<string, string> _files;

        /// <summary>
        /// saves files in //data/
        /// </summary>
        public FileHandler() 
        {
            _files = new Dictionary<string, string>();
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
            IFileEncoder fileEncoder = new DLLEncoder();
            string encoding = fileEncoder.GetEncoded( dllFiles.ToList() );
            Trace.Write( dllFiles );
            Trace.Write( encoding );
            Trace.Assert( fileEncoder != null );

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
