using Networking.Communicator;
using Content.FileHandling;
using Content.Encoder;
using Analyzer;

namespace Content.Server
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

        public Action<List<AnalyzerResult>> AnalyzerResultChanged;

        public List<AnalyzerResult >analyzerResult {  get; private set; }

        /// <summary>
        /// Initialise the content server, subscribe to networking server
        /// </summary>
        /// <param name="_server">Networking server</param>
        /// <param name="_analyzer">Analyzer</param>
        public ContentServer(ICommunicator _server, IAnalyzer _analyzer) 
        {
            server = _server;
            ServerRecieveHandler recieveHandler = new ServerRecieveHandler(this);
            server.Subscribe(recieveHandler, "Content");

            fileHandler = new FileHandler(server);

            analyzer = _analyzer;

            // Currently hardcoded, uses all analyzers
            Dictionary<int, bool> configuration = new Dictionary<int, bool>();
            configuration[103] = true; // TODO TODO
            analyzer.Configure(configuration, true);

            serializer = new AnalyzerResultSerializer();
        }

        /// <summary>
        /// Handles a recieved file by decoding and saving it, and then passing it to the analyser
        /// </summary>
        /// <param name="encodedFiles">The encoded file</param>
        /// <param name="clientID">Unique ID of client</param>
        public void HandleRecieve(string encodedFiles, string? clientID)
        {
            // Save files to user session directory
            fileHandler.HandleRecieve(encodedFiles);

            // Analyse DLL files
            analyzer.LoadDLLFile(fileHandler.GetFiles(), null);

            // Save analysis results 
            analyzerResult = analyzer.Run();

            // Send Analysis results to client
            //server.Send(serializer.Serialize(analyzerResult), EventType.AnalyserResult(), clientID);
            // ClientID is currently not implemented

            // Notification for viewModel
            AnalyzerResultChanged?.Invoke(analyzerResult);
        }



    }
}
