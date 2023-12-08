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
using Logging;
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
        public enum StatusType
        {
            NONE,
            WAITING,
            SUCCESS,
            FAILURE
        }
        StatusType _status;
        /// <summary>
        /// Output of analysis
        /// </summary>
        public Dictionary<string , List<AnalyzerResult>> analyzerResult { get; private set; }

        /// <summary>
        /// Action to be invoked when <see cref="analyzerResult"/> is changed
        /// </summary>
        public Action<Dictionary<string , List<AnalyzerResult>>>? AnalyzerResultChanged;

        /// <summary>
        /// Invoked when client status changes
        /// </summary>
        public Action<StatusType>? ClientStatusChanged;


        /// <summary>
        /// Initializes a new instance of the ContentClient class.
        /// </summary>
        public ContentClient( ICommunicator client , string sessionID )
        {
            _client = client;
            ClientRecieveHandler recieveHandler = new( this );
            _client.Subscribe( recieveHandler , "Content-Results" );
            _client.Subscribe( recieveHandler , "Content-Messages" );
            _fileHandler = new FileHandler();
            _sessionID = sessionID;
            _serializer = new AnalyzerResultSerializer();
            _status = StatusType.NONE;
            analyzerResult = new();
            Logger.Inform( "[ContentClient.cs] ContentClient: Initialized ContentClient" );
        }

        internal void ContentMessageInfo( string data )
        {
            Logger.Inform( "[ContentClient.cs] ContentMessageInfo: Started " );
            if (data == "Success")
            {
                Logger.Inform( "[ContentClient.cs] ContentMessageInfo: Sucess Message " );
                SetStatus( StatusType.SUCCESS );
            }
            else if (data == "Failure")
            {
                Logger.Inform( "[ContentClient.cs] ContentMessageInfo: Failure Message " );
                SetStatus( StatusType.FAILURE );
            }
            else
            {
                Logger.Inform( "[ContentClient.cs] ContentMessageInfo: Invalid Message " );
            }
            Logger.Inform( "[ContentClient.cs] ContentMessageInfo: Done " );
        }

        private void SetStatus( StatusType status )
        {
            _status = status;
            ClientStatusChanged?.Invoke( _status );
            Logger.Debug( $"[ContentClient.cs] SetStatus: {_status}" );
        }



        /// <summary>
        /// Handles the upload of files from a folder/file to the folder specified for that session.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing files to upload 
        /// or path to the file to upload</param>
        public void HandleUpload( string folderPath )
        {
            Logger.Inform( "[ContentClient.cs] HandleUpload: Started" );
            SetStatus( StatusType.WAITING );
            string encoding = _fileHandler.HandleUpload( folderPath , _sessionID );
            _client.Send( encoding , "Content-Files" , "server" );
            Logger.Inform( "[ContentClient.cs] HandleUpload: Done" );
        }

        /// <summary>
        /// Handles the received analyzer results encoded data.
        /// </summary>
        /// <param name="encoding">The encoded data containing analyzer results.</param>
        public void HandleReceive( string encoding )
        {
            Logger.Inform( "[ContentClient.cs] HandleReceive: Started" );
            analyzerResult = _serializer.Deserialize<Dictionary<string , List<AnalyzerResult>>>( encoding );
            AnalyzerResultChanged?.Invoke( analyzerResult );
            Logger.Inform( "[ContentClient.cs] HandleReceive: Done" );
        }
    }
}
