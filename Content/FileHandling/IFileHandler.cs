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
<<<<<<< HEAD
        string HandleUpload(string filepath, string sessionID);
=======
        string HandleSend(string filepath, string sessionID);
>>>>>>> 7266298251d67cc4671e44d6712a39832b4682c5

        /// <summary>
        /// Handle a received file by saving it to a directory
        /// </summary>
        /// <param name="sessionID">ID of this session</param>
        /// <returns></returns>
        void HandleRecieve(string encoding);
        List<string> GetFiles();
    }
}
