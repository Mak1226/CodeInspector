using Networking.Communicator;
using Content.FileHandling;
using Content.Encoder;
using Analyzer;
using System.Threading.Tasks;

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

        public Action<Dictionary<string, List<AnalyzerResult>>>? AnalyzerResultChanged;

        public Dictionary<string, List<AnalyzerResult>> analyzerResult { get; private set; }

        private readonly Dictionary<string, Dictionary<string, List<AnalyzerResult>>> _sessionAnalysisResultDict;
        private readonly object _sessionLock = new ();

        /// <summary>
        /// Initialise the content server, subscribe to networking server
        /// </summary>
        /// <param name="_server">Networking server</param>
        /// <param name="_analyzer">Analyzer</param>
        public ContentServer(ICommunicator _server, IAnalyzer _analyzer)
        {
            this._server = _server;
            _hostSessionID = "";
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
            _fileEncoding = encodedFiles;
            // Save files to user session directory and collect sessionID
            string? recievedSessionID = _fileHandler.HandleRecieve(encodedFiles);
            if (recievedSessionID == null) 
            { 
                return; // FileHandler failed
            }

            // Analyse DLL files
            _analyzer.LoadDLLFileOfStudent(_fileHandler.GetFiles());

            // Send Analysis results to client

            // Save analysis results 
            lock (_sessionLock)
            {
                Dictionary<string , List<AnalyzerResult>> res = _analyzer.Run();

                Dictionary<string, List<AnalyzerResult>> customRes = _analyzer.RnuCustomAnalyzers();
                res[""] = new(); //blank line as separator
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
                    analyzerResult = res;
                    // Notification for viewModel
                    AnalyzerResultChanged?.Invoke(analyzerResult);
                }
            }

        }

        public void Configure(IDictionary<int, bool> configuration)
        {
            _configuration = configuration;
            _analyzer.Configure(configuration);
        }

        public void LoadCustomDLLs(List<string> filePaths)
        {
            _analyzer.LoadDLLOfCustomAnalyzers(filePaths);
        }

        public void SetSessionID(string? sessionID)
        {
            if (sessionID == null)
            {
                _sessionID = null;
                return;
            }


            _sessionID = sessionID;
            if (_sessionAnalysisResultDict.ContainsKey(sessionID)) 
            {
                analyzerResult = _sessionAnalysisResultDict[sessionID];
            }
            else
            {
                analyzerResult = new();
                _sessionAnalysisResultDict[sessionID] = analyzerResult;
            }
            AnalyzerResultChanged?.Invoke(analyzerResult);
        }

        public void SendToCloud()
        {
            CloudHandler cloudHandler = new CloudHandler();
            Task.Run(() 
                => cloudHandler.PostSessionAsync(_hostSessionID, _configuration, _sessionAnalysisResultDict.Keys.ToList()));
            Task.Run(()
                => cloudHandler.PostSubmissionAsync(_hostSessionID, _fileEncoding));
            Task.Run(()
                => cloudHandler.PostSubmissionAsync(_hostSessionID, _resultEncoding));
        }

    }
}
