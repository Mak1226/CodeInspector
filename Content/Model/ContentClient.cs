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

using Analyzer;
using Content.Encoder;
using Content.FileHandling;
using Networking.Communicator;

namespace Content.Model
{
    /// <summary>
    /// Class that handles communication between ContentPage and Content
    /// </summary>
    public class ContentClient
    {
        readonly ICommunicator _client;
        readonly IFileHandler _fileHandler;
        readonly string _sessionID;
        readonly AnalyzerResultSerializer _serializer;

        public Dictionary<string, List<AnalyzerResult>> analyzerResult { get; private set; }
        public Action<Dictionary<string, List<AnalyzerResult>>>? AnalyzerResultChanged;

        /// <summary>
        /// Initializes a new instance of the ContentClient class.
        /// </summary>
        public ContentClient(ICommunicator client, string sessionID)
        {
            _client = client;
            ClientRecieveHandler recieveHandler = new (this);
            _client.Subscribe(recieveHandler, "Content-Results");

            _fileHandler = new FileHandler();
            _sessionID = sessionID;
            _serializer = new AnalyzerResultSerializer();

            analyzerResult = new();
        }

        public ContentClient()
        {
        }

        /// <summary>
        /// Handles the upload of files from a folder to the folder specified for that session.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing files to upload.</param>
        public void HandleUpload(string folderPath)
        {
            string encoding = _fileHandler.HandleUpload(folderPath, _sessionID);
            _client.Send(encoding, "Content-Files", "server");
        }

        public void HandleReceive(string encoding)
        {
            analyzerResult = _serializer.Deserialize<Dictionary<string, List<AnalyzerResult>>>(encoding);
            AnalyzerResultChanged?.Invoke(analyzerResult);
        }
    }
}
