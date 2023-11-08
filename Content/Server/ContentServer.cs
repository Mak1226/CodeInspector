using Networking.Communicator;
using Networking;
using Content.FileHandling;
using Content.Encoder;
using Analyzer;

namespace Content.Server
{
    using AnalyzerResult = Tuple<Dictionary<string, string>, int>;

    /// <summary>
    /// Server that handles upload of analysis files, and analysis of said files
    /// </summary>
    public class ContentServer
    {
        ICommunicator server;
        IFileHandler fileHandler;
        IAnalyzer analyzer;
        AnalyzerResultSerializer serializer;

        public Action<AnalyzerResult> AnalyzerResultChanged;

        public AnalyzerResult analyzerResult {  get; private set; }

        /// <summary>
        /// Initializes networking, filehandling and analyser
        /// </summary>
        public ContentServer() 
        {
            server = CommunicationFactory.GetCommunicator(true);
            ServerRecieveHandler recieveHandler = new ServerRecieveHandler(this);
            server.Subscribe(recieveHandler, "Content");

            fileHandler = new FileHandler(server);

            analyzer = AnalyzerFactory.GetAnalyser();

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
            analyzerResult = analyzer.GetAnalysis();

            // Send Analysis results to client
            //server.Send(serializer.Serialize(analyzerResult), EventType.AnalyserResult(), clientID);
            // ClientID is currently not implemented

            // Notification for viewModel
            AnalyzerResultChanged?.Invoke(analyzerResult);
        }



    }
}
