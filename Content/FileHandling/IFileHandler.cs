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

        /// <summary>
        /// Upload a file to encoded as an XML
        /// </summary>
        /// <param name="filepath">path to file</param>
        /// <param name="sessionID">ID of this session</param>
        /// <returns>Encoded string containing the files and other details encoded</returns>
        string HandleUpload(string filepath, string sessionID);

        /// <summary>
        /// Handle a received file by saving it to a directory
        /// </summary>
        /// <param name="encoding">The XML encoded string of the files</param>
        /// <returns>The session ID of the recieved message. Null if unable to recieve</returns>
        string? HandleRecieve(string encoding);
        List<string> GetFiles();
    }
}
