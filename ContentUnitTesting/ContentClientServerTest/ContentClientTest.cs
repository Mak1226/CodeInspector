using Analyzer;
using Content.Encoder;
using Content.FileHandling;
using Content.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContentUnitTesting.ContentClientServerTest
{
    [TestClass]
    public class ContentClientTest
    {
        MockCommunicator _communicator;

        [TestInitialize]
        public void TestInitializer()
        {
            _communicator = new MockCommunicator();
        }
        [TestMethod]
        public void FileSendTest()
        {
            ContentClient contentClient = new ContentClient(_communicator, "testSessionID");
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            Directory.CreateDirectory(tempDirectory + "\\subdir1");
            File.WriteAllText(Path.Combine(tempDirectory + "\\subdir1", "TestDll3.dll"), "DLL Content 3");

            contentClient.HandleUpload(tempDirectory);
            IFileHandler fileHandler = new FileHandler();
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");
            Assert.AreEqual(encoding, _communicator.GetEncoding());
            Directory.Delete(tempDirectory, true);
        }
        [TestMethod]
        public void FileReceiveTest()
        {
            ContentClient contentClient = new ContentClient(_communicator, "currSession");
            Dictionary<string, List<AnalyzerResult>> analyzerResultUpdated = new Dictionary<string, List<AnalyzerResult>>();
            contentClient.AnalyzerResultChanged += (result) =>
            {
                analyzerResultUpdated = result;
            };
            Dictionary<string, List<AnalyzerResult>> analyzerResult = new Dictionary<string, List<AnalyzerResult>>
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
                // Add more initial values as needed
            };
            AnalyzerResultSerializer serializer = new AnalyzerResultSerializer();
            string encoding = serializer.Serialize(analyzerResult);
            contentClient.HandleReceive(encoding);
            Assert.IsTrue(analyzerResultUpdated.ContainsKey("File1"));
        }
        [TestMethod]
        public void NullReceiveTest()
        {
            ContentClient contentClient = new ContentClient(_communicator, "currSession");

            Dictionary<string, List<AnalyzerResult>> analyzerResult = new Dictionary<string, List<AnalyzerResult>>
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
                // Add more initial values as needed
            };
            AnalyzerResultSerializer serializer = new AnalyzerResultSerializer();
            Dictionary<string, List<AnalyzerResult>> analyzerResultUpdated = new Dictionary<string, List<AnalyzerResult>>();
            string encoding = serializer.Serialize(analyzerResultUpdated);
            contentClient.HandleReceive(encoding);
            // When passed value is empty, analyzerResult is not updated
            Assert.IsTrue(analyzerResult.ContainsKey("File1"));
        }
    }
}
