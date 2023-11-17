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
        ICommunicator server;
        IFileHandler fileHandler;
        IAnalyzer analyzer;
        AnalyzerResultSerializer serializer;
        string? sessionID;

        public Action<Dictionary<string, List<AnalyzerResult>>>? AnalyzerResultChanged;

        public Dictionary<string, List<AnalyzerResult>> analyzerResult { get; private set; }
        private Dictionary<string, Dictionary<string, List<AnalyzerResult>>> sessionAnalysisResultDict;

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
            if (sessionID == null)
            {
                return; //If no session is loaded, don't do anything
            }

            // Save files to user session directory
            fileHandler.HandleRecieve(encodedFiles);

            // Analyse DLL files
            analyzer.LoadDLLFileOfStudent(fileHandler.GetFiles());

            // Save analysis results 
            analyzerResult = analyzer.Run();
            sessionAnalysisResultDict[sessionID] = analyzerResult;

            // Send Analysis results to client
            server.Send(serializer.Serialize(analyzerResult), "Content-Results", clientID);

            // Notification for viewModel
            AnalyzerResultChanged?.Invoke(analyzerResult);
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
