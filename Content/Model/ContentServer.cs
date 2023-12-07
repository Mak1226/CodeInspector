/******************************************************************************
 * Filename    = ContentServer.cs
 * 
 * Author      = Lekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Class that represents a client for handling file uploads.
 *****************************************************************************/

using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using Analyzer;
using Content.Encoder;
using Content.FileHandling;
using Logging;
using Networking.Communicator;
using Networking.Serialization;

namespace Content.Model
{

    /// <summary>
    /// Server that handles upload of analysis files, and analysis of said files
    /// </summary>
    public class ContentServer
    {
        static readonly string s_configurationPath = "teacher_configuration.json";

        readonly ICommunicator _server;
        readonly string _hostSessionID;
        readonly IAnalyzer _analyzer;

        string? _sessionID;

        IDictionary<int , bool> _configuration;
        internal IDictionary<int , bool> Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    if (File.Exists( s_configurationPath ))
                    {
                        Logger.Inform( "[ContentServer.cs] Loading Configuration" );
                        string json = File.ReadAllText( s_configurationPath );
                        IDictionary<int , bool>? result = JsonSerializer.Deserialize<IDictionary<int , bool>>( json );
                        if (result != null)
                        {
                            _configuration = result;

                            // Filter out custom analyzers
                            List<int> keysToRemove = _configuration.Keys.Where( x => x >= 200 ).ToList();
                            foreach (int keyToRemove in keysToRemove)
                            {
                                _configuration.Remove( keyToRemove );
                            }

                            return _configuration;
                        }
                    }
                    return new Dictionary<int , bool>();
                }
                return _configuration;
            }

            set
            {
                Logger.Inform( "[ContentServer.cs] Saving Configuration" );
                _configuration = value;
                string json = JsonSerializer.Serialize( _configuration );
                File.WriteAllText( s_configurationPath , json );
            }
        }

        /// <summary>
        /// Delegate Function called when <see cref="analyzerResult"/> is changed
        /// </summary>
        public Action<Dictionary<string , List<AnalyzerResult>>>? AnalyzerResultChanged;

        /// <summary>
        /// Currently loaded Analyzer Result
        /// </summary>
        public Dictionary<string , List<AnalyzerResult>> analyzerResult { get; private set; }

        private readonly Dictionary<string , Dictionary<string , List<AnalyzerResult>>> _sessionAnalysisResultDict;
        private readonly object _sessionLock = new();

        /// <summary>
        /// Initialise the content server, subscribe to networking server
        /// </summary>
        /// <param name="_server">Networking server</param>
        /// <param name="_analyzer">Analyzer</param>
        public ContentServer( ICommunicator _server , IAnalyzer _analyzer , string sessionID )
        {
            this._server = _server;
            _hostSessionID = sessionID;
            ServerRecieveHandler recieveHandler = new( this );
            this._server.Subscribe( recieveHandler , "Content-Files" );
            this._analyzer = _analyzer;
            this._analyzer.Configure( Configuration );
            analyzerResult = new();
            _sessionAnalysisResultDict = new();
            Logger.Inform( "[ContentServer.cs] ContentServer: Initialized ContentServer" );
        }

        /// <summary>
        /// Handles a recieved file by decoding and saving it, and then passing it to the analyser
        /// </summary>
        /// <param name="encodedFiles">The encoded file</param>
        /// <param name="clientID">Unique ID of client</param>
        public void HandleRecieve( string encodedFiles , string? clientID )
        {
            Logger.Inform( "[ContentServer.cs] HandleReceive: Started" );
            IFileHandler _fileHandler = new FileHandler();
            ISerializer _serializer = new AnalyzerResultSerializer();

            // Save files to user session directory and collect sessionID
            string? recievedSessionID = _fileHandler.HandleRecieve( encodedFiles );
            if (recievedSessionID == null)
            {
                Logger.Warn( "[ContentServer.cs] HandleReceive: unable to recieve" );
                _server.Send( "Faliure" , "Content-Messages" , clientID );
                return; // FileHandler failed
            }
            else
            {
                Logger.Debug( $"[ContentServer.cs] HandleReceive: Recieved sessionID {recievedSessionID}" );
                _server.Send( "Success" , "Content-Messages" , clientID );
            }

            // Save analysis results 
            lock (_sessionLock)
            {
                Logger.Debug( "[ContentServer.cs] HandleReceive: Inside SessionLock" );
                // Analyse DLL files
                _analyzer.LoadDLLFileOfStudent( _fileHandler.GetFiles() );
                Logger.Debug( $"[ContentServer.cs] HandleReceive: Loaded Student DLL files" );
                Dictionary<string , List<AnalyzerResult>> res = _analyzer.Run();

                _sessionAnalysisResultDict[recievedSessionID] = res;
                string serializedResults = _serializer.Serialize( res );
                _server.Send( serializedResults , "Content-Results" , clientID );
                Logger.Debug( $"[ContentServer.cs] HandleReceive: Sending result {serializedResults}" );
                if (_sessionID == recievedSessionID)
                {
                    Logger.Debug( $"[ContentServer.cs] HandleReceive: SessionIDs match ({_sessionID})" );
                    analyzerResult = res;
                    // Notification for viewModel
                    AnalyzerResultChanged?.Invoke( analyzerResult );
                }

                byte[] graph = _analyzer.GetRelationshipGraph( new() );
                if (graph == null || graph.Length == 0)
                {
                    Logger.Warn( "[ContentServer.cs] HandleReceive: Graph is either null or of 0 length" );
                    return;
                }

                // Save the image as PNG
                try
                {
                    using MemoryStream ms = new( graph );
                    Image image = Image.FromStream( ms );
                    image.Save( recievedSessionID + "/image.png" , ImageFormat.Png );
                    Logger.Debug( $"[ContentServer.cs] HandleReceive: Successfully saved graph for {recievedSessionID}" );
                }
                catch (Exception ex)
                {
                    Logger.Error( $"[ContentServer.cs] HandleReceive : Couldn't save graph for {recievedSessionID}. {ex}" );
                }
            }
            Logger.Inform( "[ContentServer.cs] HandleReceive: Done" );

        }
        /// <summary>
        /// Configures the analyzer with the specified configuration settings.
        /// </summary>
        /// <param name="configuration">The dictionary containing configuration settings.</param>
        public void Configure( IDictionary<int , bool> configuration )
        {
            Logger.Inform( "[ContentServer.cs] Configure: Started" );
            Configuration = configuration;
            _analyzer.Configure( configuration );
            Logger.Inform( "[ContentServer.cs] Configure: Done" );
        }

        /// <summary>
        /// Loads custom DLLs for additional analyzers.
        /// </summary>
        /// <param name="filePaths">The list of file paths for the custom DLLs.</param>
        public List<Tuple<int , string>> LoadCustomDLLs( List<string> filePaths )
        {
            Logger.Inform( $"[ContentServer.cs] LoadCustomDLLs {filePaths}" );
            return _analyzer.LoadDLLOfCustomAnalyzers( filePaths );
        }

        /// <summary>
        /// Sets the session ID and updates the associated analyzer results if available.
        /// </summary>
        /// <param name="sessionID">The session ID to be set.</param>
        public void SetSessionID( string? sessionID )
        {
            Logger.Inform( $"[ContentServer.cs] SetSessionID: started. SessionID {sessionID}" );
            if (sessionID == null)
            {

                Logger.Warn( "[ContentServer.cs] SetSessionID: sessionID is null" );
                _sessionID = null;
                return;
            }

            _sessionID = sessionID;
            if (_sessionAnalysisResultDict.ContainsKey( sessionID ))
            {
                Logger.Debug( "[ContentServer.cs] SetSessionID: Already sessionID present" );
                // Use existing analyzer results if available for the given session ID.
                analyzerResult = _sessionAnalysisResultDict[sessionID];
            }
            else
            {

                Logger.Debug( "[ContentServer.cs] SetSessionID: new SessionID" );
                // Create a new entry for the session ID if not present.
                analyzerResult = new();
                _sessionAnalysisResultDict[sessionID] = analyzerResult;
            }
            AnalyzerResultChanged?.Invoke( analyzerResult );
            Logger.Inform( "[ContentServer.cs] SetSessionID: done" );
        }

        /// <summary>
        /// Funciton to cumulate all data and send them to cloud.
        /// </summary>
        public void SendToCloud()
        {

            Logger.Inform( "[ContentServer.cs] SentToCloud: started" );
            CloudHandler cloudHandler = new();
            Logger.Debug( $"[ContentServer.cs] SentToCloud : Session :: {_hostSessionID}" );
            _ = cloudHandler.PostSessionAsync( _hostSessionID , _configuration , _sessionAnalysisResultDict.Keys.ToList() );
            foreach (KeyValuePair<string , Dictionary<string , List<AnalyzerResult>>> kvp in _sessionAnalysisResultDict)
            {
                Logger.Debug( $"[ContentServer.cs] SentToCloud : Analysis :: {kvp.Key}" );
                _ = cloudHandler.PostAnalysisAsync( kvp.Key, kvp.Value);

                IFileHandler fileHandler = new FileHandler();
                string encoding = fileHandler.HandleUpload(kvp.Key, kvp.Key);
                Logger.Debug( $"[ContentServer.cs] SentToCloud : Submission :: {kvp.Key}" );
                _ = cloudHandler.PostSubmissionAsync( kvp.Key , encoding );
            }
            Logger.Inform( "[ContentServer.cs] SentToCloud: done" );
        }

    }
}
