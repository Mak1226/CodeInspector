using Networking.Communicator;
using Content.FileHandling;
using Content.Encoder;
using Analyzer;

namespace Content.Model
{

    /// <summary>
    /// Server that handles upload of analysis files, and analysis of said files
    /// </summary>
    public class ContentServer
    {
        readonly ICommunicator server;
        readonly IFileHandler fileHandler;
        readonly IAnalyzer analyzer;
        readonly AnalyzerResultSerializer serializer;
        
        string? sessionID;

        public Action<Dictionary<string, List<AnalyzerResult>>>? AnalyzerResultChanged;

        public Dictionary<string, List<AnalyzerResult>> analyzerResult { get; private set; }
        private readonly Dictionary<string, Dictionary<string, List<AnalyzerResult>>> sessionAnalysisResultDict;

        /// <summary>
        /// Initialise the content server, subscribe to networking server
        /// </summary>
        /// <param name="_server">Networking server</param>
        /// <param name="_analyzer">Analyzer</param>
        public ContentServer(ICommunicator _server, IAnalyzer _analyzer)
        {
            server = _server;
            ServerRecieveHandler recieveHandler = new ServerRecieveHandler(this);
            server.Subscribe(recieveHandler, "Content-Files");

            fileHandler = new FileHandler();

            analyzer = _analyzer;

            serializer = new AnalyzerResultSerializer();

            analyzerResult = new();
            sessionAnalysisResultDict = new();
        }

        /// <summary>
        /// Handles a recieved file by decoding and saving it, and then passing it to the analyser
        /// </summary>
        /// <param name="encodedFiles">The encoded file</param>
        /// <param name="clientID">Unique ID of client</param>
        public void HandleRecieve(string encodedFiles, string? clientID)
        {
            // Save files to user session directory and collect sessionID
            string? recievedSessionID = fileHandler.HandleRecieve(encodedFiles);
            if (recievedSessionID == null) 
            { 
                return; // FileHandler failed
            }

            // Analyse DLL files
            analyzer.LoadDLLFileOfStudent(fileHandler.GetFiles());

            // Send Analysis results to client
            server.Send(serializer.Serialize(analyzerResult), "Content-Results", clientID);

            // Save analysis results 
            sessionAnalysisResultDict[recievedSessionID] = analyzer.Run();
            if (sessionID == recievedSessionID)
            {
                analyzerResult = sessionAnalysisResultDict[sessionID];
                // Notification for viewModel
                AnalyzerResultChanged?.Invoke(analyzerResult);
            }

        }

        public void Configure(IDictionary<int, bool> configuration)
        {
            analyzer.Configure(configuration);
        }

        public void SetSessionID(string? sessionID)
        {
            if (sessionID == null)
            {
                this.sessionID = null;
                return;
            }


            this.sessionID = sessionID;
            if (sessionAnalysisResultDict.ContainsKey(sessionID)) 
            {
                analyzerResult = sessionAnalysisResultDict[sessionID];
            }
            else
            {
                analyzerResult = new();
                sessionAnalysisResultDict[sessionID] = analyzerResult;
            }
            AnalyzerResultChanged?.Invoke(analyzerResult);
        }

    }
}
