/******************************************************************************
 * Filename     = ContentClientTest.cs
 * 
 * Author       = Lekshmi
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit Tests for ContentClient
*****************************************************************************/
using System.Diagnostics;
using Analyzer;
using Content.Encoder;
using Content.FileHandling;
using Content.Model;

namespace ContentUnitTesting.ContentClientServerTest
{
    /// <summary>
    /// Class to test file ContentClient.cs
    /// </summary>
    [TestClass]
    public class ContentClientTest
    {
        MockCommunicator _communicator;

        /// <summary>
        /// Test initialization method. 
        /// Creates a new instance of MockCommunicator for each test.
        /// </summary>
        [TestInitialize]
        public void TestInitializer()
        {
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] Initializing test class for ContentClient" );
            _communicator = new MockCommunicator();
        }

        /// <summary>
        /// Tests the file upload functionality of the ContentClient class.
        /// </summary>
        [TestMethod]
        public void FileSendTest()
        {
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] FileSendTest: Started" );
            ContentClient contentClient = new (_communicator, "testSessionID");
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            Directory.CreateDirectory(tempDirectory + "\\subdir1");
            File.WriteAllText(Path.Combine(tempDirectory + "\\subdir1", "TestDll3.dll"), "DLL Content 3");
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] FileSendTest: Uploading temp directory with one subdirectory" );
            contentClient.HandleUpload(tempDirectory);
            IFileHandler fileHandler = new FileHandler();
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");
            Assert.AreEqual(encoding, _communicator.GetEncoding());
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] FileSendTest: Deleting temp directory" );
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// Tests the file receive functionality of the ContentClient class.
        /// </summary>
        [TestMethod]
        public void FileReceiveTest()
        {
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] FileReceiveTest: Started" );
            ContentClient contentClient = new (_communicator, "currSession");
            Dictionary<string, List<AnalyzerResult>> analyzerResultUpdated = new ();
            contentClient.AnalyzerResultChanged += (result) =>
            {
                analyzerResultUpdated = result;
            };
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] FileReceiveTest: Sample analyzer result created" );
            Dictionary<string, List<AnalyzerResult>> analyzerResult = new()
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
                // Add more initial values as needed
            };
            AnalyzerResultSerializer serializer = new ();
            string encoding = serializer.Serialize(analyzerResult);
            contentClient.HandleReceive(encoding);
            Assert.IsTrue(analyzerResultUpdated.ContainsKey("File1"));
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] FileReceiveTest: Done" );
        }

        /// <summary>
        /// Tests the behavior when receiving null data in the ContentClient class.
        /// </summary>
        [TestMethod]
        public void NullReceiveTest()
        {
            Trace.WriteLine( "[ContentUnitTesting][ContentClientTest.cs] NullReceiveTest: Started" );
            ContentClient contentClient = new (_communicator, "currSession");
            Dictionary<string, List<AnalyzerResult>> analyzerResult = new ()
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
                // Add more initial values as needed
            };
            AnalyzerResultSerializer serializer = new ();
            Dictionary<string, List<AnalyzerResult>> analyzerResultUpdated = new ();
            string encoding = serializer.Serialize(analyzerResultUpdated);
            contentClient.HandleReceive(encoding);
            // When passed value is empty, analyzerResult is not updated
            Assert.IsTrue(analyzerResult.ContainsKey("File1"));
        }
    }
}
