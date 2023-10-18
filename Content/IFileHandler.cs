/******************************************************************************
* Author      = Susan
*
* Product     = Analyzer
* 
* Project     = Content
*
* Description = Interface for handling file upload and download
*****************************************************************************/

namespace Content
{
    /// <summary>
    /// Interface to upload and download files to and from server
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Upload a file from server
        /// </summary>
        /// <param name="filepath">path to file</param>
        /// <param name="sessionID">ID of this session</param>
        void Upload(string filepath, string sessionID);

        /// <summary>
        /// Download a file from the server, pressumably queued for this session
        /// </summary>
        /// <param name="sessionID">ID of this session</param>
        /// <returns></returns>
        string Download(string sessionID);
    }
}
