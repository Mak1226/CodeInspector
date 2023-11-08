using Networking.Communicator;
using Networking;
using Content.FileHandling;
using Content.Encoder;
using Analyzer;

namespace Content.Server
{
    using AnalyzerResult = Tuple<Dictionary<string, string>, int>;
    public class ContentServer
    {
        ICommunicator server;
        IFileHandler fileHandler;
        IAnalyzer analyzer;
        AnalyzerResultSerializer serializer;

        public AnalyzerResult? analyzerResult {  get; private set; }

        public ContentServer() 
        {
            server = CommunicationFactory.GetCommunicator(true);
            ServerRecieveHandler recieveHandler = new ServerRecieveHandler(this);
            server.Subscribe(recieveHandler, "Content");

            fileHandler = new FileHandler(server);

            analyzer = AnalyzerFactory.GetAnalyser();

            serializer = new AnalyzerResultSerializer();
        }

        public void HandleRecieve(string encodedFiles, string? clientID)
        {
            // Save files to user session directory
            fileHandler.HandleRecieve("Dummy", encodedFiles);

            // Analyse DLL files
            analyzer.LoadDLLFile(fileHandler._filesList, null);

            // Save analysis results 
            analyzerResult = analyzer.GetAnalysis();

            // Send Analysis results to client
            server.Send(serializer.Serialize(analyzerResult), EventType.AnalyserResult(), clientID);
        }
    }
}
