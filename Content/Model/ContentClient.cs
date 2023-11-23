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

using System.Diagnostics;
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

        /// <summary>
        /// Output of analysis
        /// </summary>
        public Dictionary<string, List<AnalyzerResult>> analyzerResult { get; private set; }

        /// <summary>
        /// Action to be invoked when <see cref="analyzerResult"/> is changed
        /// </summary>
        public Action<Dictionary<string, List<AnalyzerResult>>>? AnalyzerResultChanged;

        /// <summary>
        /// Initializes a new instance of the ContentClient class.
        /// </summary>
        public ContentClient(ICommunicator client, string sessionID)
        {
            Trace.WriteLine( "[Content][ContentClient.cs] ContentClient: Initialized ContentClient" );
            _client = client;
            ClientRecieveHandler recieveHandler = new (this);
            _client.Subscribe(recieveHandler, "Content-Results");

            _fileHandler = new FileHandler();
            _sessionID = sessionID;
            _serializer = new AnalyzerResultSerializer();

            analyzerResult = new();
        }

        /// <summary>
        /// Handles the upload of files from a folder/file to the folder specified for that session.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing files to upload 
        /// or path to the file to upload</param>
        public void HandleUpload(string folderPath)
        {
            Trace.WriteLine( "[Content][ContentClient.cs] HandleUpload: Started" );
            string encoding = _fileHandler.HandleUpload(folderPath, _sessionID);
            _client.Send(encoding, "Content-Files", "server");
            Trace.WriteLine( "[Content][ContentClient.cs] HandleUpload: Started" );
        }

        /// <summary>
        /// Handles the received analyzer results encoded data.
        /// </summary>
        /// <param name="encoding">The encoded data containing analyzer results.</param>
        public void HandleReceive(string encoding)
        {
            Trace.WriteLine( "[Content][ContentClient.cs] HandleReceive: Started" );
            analyzerResult = _serializer.Deserialize<Dictionary<string, List<AnalyzerResult>>>(encoding);
            AnalyzerResultChanged?.Invoke(analyzerResult);
            Trace.WriteLine( "[Content][ContentClient.cs] HandleReceive: Started" );
        }
    }
}
