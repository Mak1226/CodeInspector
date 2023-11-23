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

using Networking.Communicator;
using Content.FileHandling;
using Content.Encoder;
using Analyzer;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Content.Model
{

    /// <summary>
    /// Server that handles upload of analysis files, and analysis of said files
    /// </summary>
    public class ContentServer
    {
        readonly ICommunicator _server;
        readonly string _hostSessionID;
        readonly IFileHandler _fileHandler;
        readonly IAnalyzer _analyzer;
        readonly AnalyzerResultSerializer _serializer;

        string? _sessionID;
        string? _fileEncoding;
        string? _resultEncoding;
        IDictionary<int, bool> _configuration;

        /// <summary>
        /// Delegate Function called when <see cref="analyzerResult"/> is changed
        /// </summary>
        public Action<Dictionary<string, List<AnalyzerResult>>>? AnalyzerResultChanged;

        /// <summary>
        /// Currently loaded Analyzer Result
        /// </summary>
        public Dictionary<string, List<AnalyzerResult>> analyzerResult { get; private set; }

        private readonly Dictionary<string, Dictionary<string, List<AnalyzerResult>>> _sessionAnalysisResultDict;
        private readonly object _sessionLock = new ();

        /// <summary>
        /// Initialise the content server, subscribe to networking server
        /// </summary>
        /// <param name="_server">Networking server</param>
        /// <param name="_analyzer">Analyzer</param>
        public ContentServer(ICommunicator _server, IAnalyzer _analyzer, string sessionID)
        {
            Trace.WriteLine( "[Content][ContentServer.cs] ContentServer: Initialized ContentServer" );
            this._server = _server;
            _hostSessionID = sessionID;
            ServerRecieveHandler recieveHandler = new (this);
            this._server.Subscribe( recieveHandler , "Content-Files");
            _fileHandler = new FileHandler();
            this._analyzer = _analyzer;
            _serializer = new AnalyzerResultSerializer();
            analyzerResult = new();
            _sessionAnalysisResultDict = new();
        }

        /// <summary>
        /// Handles a recieved file by decoding and saving it, and then passing it to the analyser
        /// </summary>
        /// <param name="encodedFiles">The encoded file</param>
        /// <param name="clientID">Unique ID of client</param>
        public void HandleRecieve(string encodedFiles, string? clientID)
        {
            Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: Started" );
            _fileEncoding = encodedFiles;
            // Save files to user session directory and collect sessionID
            string? recievedSessionID = _fileHandler.HandleRecieve(encodedFiles);
            if (recievedSessionID == null)
            {
                Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: receivedSessionID is null" );
                return; // FileHandler failed
            }
            // Analyse DLL files
            _analyzer.LoadDLLFileOfStudent(_fileHandler.GetFiles());
            Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: Loaded Student DLL files" );
            // Save analysis results 
            lock (_sessionLock)
            {
                Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: Inside SessionLock" );
                Dictionary<string , List<AnalyzerResult>> res = _analyzer.Run();
                Dictionary<string, List<AnalyzerResult>> customRes = _analyzer.RnuCustomAnalyzers();
                foreach (KeyValuePair<string, List<AnalyzerResult>> kvp in customRes)
                {
                    res[kvp.Key] = res[kvp.Key].Concat(kvp.Value).ToList();
                }

                _sessionAnalysisResultDict[recievedSessionID] = res;
                string serializedResults = _serializer.Serialize(res);
                _resultEncoding = serializedResults;
                _server.Send(serializedResults, "Content-Results", clientID);
                if (_sessionID == recievedSessionID)
                {
                    Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: SessionIDs match" );
                    analyzerResult = res;
                    // Notification for viewModel
                    AnalyzerResultChanged?.Invoke(analyzerResult);
                }

                byte[] graph = _analyzer.GetRelationshipGraph(new());
                if (graph == null || graph.Length == 0)
                {
                    Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: Graph is either null or of 0 length" );
                    return;
                }

                using MemoryStream ms = new(graph);
                Image image = Image.FromStream( ms );


                // Save the image as PNG
                image.Save( recievedSessionID + "/image.png" , ImageFormat.Png );
                Trace.WriteLine( "[Content][ContentServer.cs] HandleReceive: Done" );
            }

        }
        /// <summary>
        /// Configures the analyzer with the specified configuration settings.
        /// </summary>
        /// <param name="configuration">The dictionary containing configuration settings.</param>
        public void Configure(IDictionary<int, bool> configuration)
        {
            Trace.WriteLine( "[Content][ContentServer.cs] Configure: Started" );
            _configuration = configuration;
            _analyzer.Configure(configuration);
            Trace.WriteLine( "[Content][ContentServer.cs] Configure: Done" );
        }

        /// <summary>
        /// Loads custom DLLs for additional analyzers.
        /// </summary>
        /// <param name="filePaths">The list of file paths for the custom DLLs.</param>
        public void LoadCustomDLLs(List<string> filePaths)
        {
            Trace.WriteLine( "[Content][ContentServer.cs] LoadCustomDLLs: Started" );
            _analyzer.LoadDLLOfCustomAnalyzers(filePaths);
            Trace.WriteLine( "[Content][ContentServer.cs] LoadCustomDLLs: Done" );
        }

        /// <summary>
        /// Sets the session ID and updates the associated analyzer results if available.
        /// </summary>
        /// <param name="sessionID">The session ID to be set.</param>
        public void SetSessionID(string? sessionID)
        {
            Trace.WriteLine( "[Content][ContentServer.cs] SetSessionID: started" );
            if (sessionID == null)
            {

                Trace.WriteLine( "[Content][ContentServer.cs] SetSessionID: sessionID is null" );
                _sessionID = null;
                return;
            }

            _sessionID = sessionID;
            if (_sessionAnalysisResultDict.ContainsKey(sessionID)) 
            {
                Trace.WriteLine( "[Content][ContentServer.cs] SetSessionID: Already sessionID present" );
                // Use existing analyzer results if available for the given session ID.
                analyzerResult = _sessionAnalysisResultDict[sessionID];
            }
            else
            {

                Trace.WriteLine( "[Content][ContentServer.cs] SetSessionID: new SessionID" );
                // Create a new entry for the session ID if not present.
                analyzerResult = new();
                _sessionAnalysisResultDict[sessionID] = analyzerResult;
            }
            AnalyzerResultChanged?.Invoke(analyzerResult);
            Trace.WriteLine( "[Content][ContentServer.cs] SetSessionID: done" );
        }

        /// <summary>
        /// Funciton to cumulate all data and send them to cloud.
        /// </summary>
        public void SendToCloud()
        {

            Trace.WriteLine( "[Content][ContentServer.cs] SentToCloud: started" );
            CloudHandler cloudHandler = new();
            Task.Run(() 
                => cloudHandler.PostSessionAsync(_hostSessionID, _configuration, _sessionAnalysisResultDict.Keys.ToList()));
            Task.Run(()
                => cloudHandler.PostSubmissionAsync(_hostSessionID, _fileEncoding));
            Task.Run(()
                => cloudHandler.PostSubmissionAsync(_hostSessionID, _resultEncoding));
            Trace.WriteLine( "[Content][ContentServer.cs] SentToCloud: done" );
        }

    }
}
