/******************************************************************************
 * Filename    = ContentClient.cs
 * 
 * Author      = Lekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Class that represents a client for handling file uploads.
 *****************************************************************************/

using Content.FileHandling;
using Networking.Communicator;

namespace Content.Client
{
    /// <summary>
    /// Class that handles communication between ContentPage and Content
    /// </summary>
    public class ContentClient
    {
        ICommunicator _client;
        IFileHandler _fileUploader;
        string _sessionID;
        /// <summary>
        /// Initializes a new instance of the ContentClient class.
        /// </summary>
        public ContentClient(ICommunicator client, string sessionID)
        {
            _client = client;
            _fileUploader = new FileHandler();
            _sessionID = sessionID;
        }
        /// <summary>
        /// Handles the upload of files from a folder to the folder specified for that session.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing files to upload.</param>
        public void HandleUpload(string folderPath)
        {
            string encoding = _fileUploader.HandleUpload(folderPath, _sessionID);
            _client.Send(encoding, "Content-Files", "server");
        }
    }
}
