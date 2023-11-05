/******************************************************************************
 * Filename    = IFileHandler.cs
 * 
 * Author      = Lekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Interface for handling file upload and download
 *****************************************************************************/

namespace Content.FileHandling
{
    /// <summary>
    /// Interface to upload and download files to and from server
    /// </summary>
    public interface IFileHandler
    {
        List<string> _filesList { get; set; }
        /// <summary>
        /// Upload a file to encoded as an XML
        /// </summary>
        /// <param name="filepath">path to file</param>
        /// <param name="sessionID">ID of this session</param>
        void Upload(string filepath, string sessionID);

        /// <summary>
        /// Handle a received file by saving it to a directory
        /// </summary>
        /// <param name="sessionID">ID of this session</param>
        /// <returns></returns>
        void HandleRecieve(string sessionID, string encoding);
    }
}
