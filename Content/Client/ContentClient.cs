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
using Networking;
using Networking.Communicator;

namespace Content.Client
{
    /// <summary>
    /// Class that handles communication between ContentPage and Content
    /// </summary>
    internal class ContentClient
    {
        ICommunicator _client;
        IFileHandler _fileUploader;
        /// <summary>
        /// Initializes a new instance of the ContentClient class.
        /// </summary>
        ContentClient()
        {
            _client = CommunicationFactory.GetCommunicator(false);
            _fileUploader = new FileHandler(_client);
        }
        /// <summary>
        /// Handles the upload of files from a folder to a specified session.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing files to upload.</param>
        /// <param name="sessionID">The session ID for the upload.</param>
        public void HandleUpload(string folderPath, string sessionID)
        {
            _fileUploader.Upload(folderPath, sessionID);
        }
    }
}
