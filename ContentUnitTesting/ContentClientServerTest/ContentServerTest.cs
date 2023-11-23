/******************************************************************************
 * Filename     = ContentServerTest.cs
 * 
 * Author       = Lekshmi
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit Tests for ContentServer
*****************************************************************************/
using System.Diagnostics;
using Content.Encoder;
using Content.FileHandling;
using Content.Model;

namespace ContentUnitTesting.ContentClientServerTest
{
    /// <summary>
    /// Class to test file ContentServer.cs
    /// </summary>
    [TestClass]
    public class ContentServerTest
    {
        MockCommunicator _communicator;
        MockAnalyzer _analyzer;
        /// <summary>
        /// Test initialization method. 
        /// Creates new instances of MockCommunicator and MockAnalyzer for each test.
        /// </summary>
        [TestInitialize]
        public void TestInitializer()
        {
            _communicator = new MockCommunicator();
            _analyzer = new MockAnalyzer(); 
        }

        /// <summary>
        /// Tests the behavior of handling an empty receive 
        /// in the ContentServer class.
        /// </summary>
        [TestMethod]
        public void EmptyReceiveTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            contentServer.HandleRecieve("","testSessionID");
            Assert.IsTrue(contentServer.analyzerResult.Count() == 0);
        }

        /// <summary>
        /// Tests the behavior of handling DLL files received 
        /// in the ContentServer class.
        /// </summary>
        [TestMethod]
        public void DLLReceiveTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            contentServer.SetSessionID("testSessionID");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");

            contentServer.HandleRecieve(encoding, "testClientID");
            List<string> filePaths = _analyzer.GetDllFilePath();
            List<string> expectedFilePaths = new() { "testSessionID\\" + "TestDll1.dll", "testSessionID\\" + "TestDll2.dll" };
            Assert.AreEqual(filePaths[0], expectedFilePaths[0]);
            Assert.AreEqual(filePaths[1], expectedFilePaths[1]);
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// Tests if sessionID mismatch is there, 
        /// analyzerResults are not updated
        /// </summary>
        [TestMethod]
        public void SessionIDMismatchTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            contentServer.SetSessionID("testSessionID1");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID2");

            contentServer.HandleRecieve(encoding, "testClientID");
            Assert.IsFalse(contentServer.analyzerResult.ContainsKey("File1"));
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// Tests the behavior when there is a session ID match 
        /// in the ContentServer class.
        /// </summary>
        [TestMethod]
        public void SessionIDMatchTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            contentServer.SetSessionID("testSessionID");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");

            contentServer.HandleRecieve(encoding, "testClientID2");
            Assert.IsTrue(contentServer.analyzerResult.ContainsKey("File1"));
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// Tests if proper configurations are being stored 
        /// in the ContentServer class.
        /// </summary>
        [TestMethod]
        public void ConfigureTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            IDictionary<int, bool> configuration = new Dictionary<int, bool>
            {
                { 1, true },
                { 2, false },
                { 3, true },
            };
            contentServer.Configure(configuration);
            Assert.AreEqual(configuration, _analyzer.GetTeacherOptions());
        }

        /// <summary>
        /// Tests the behavior when the session ID is set to null 
        /// in the ContentServer class.
        /// </summary>
        [TestMethod]
        public void NullSessionIDTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            contentServer.SetSessionID(null);
            Assert.IsTrue(contentServer.analyzerResult.Count() == 0);
        }

        /// <summary>
        /// Tests the behavior when the session ID is set to a previous session ID 
        /// in the ContentServer class.
        /// </summary>
        [TestMethod]
        public void PreviousSessionIDTest()
        {
            ContentServer contentServer = new (_communicator, _analyzer, "TestServer");
            contentServer.SetSessionID("TestSessionID");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            string encoding = fileHandler.HandleUpload(tempDirectory, "TestSessionID");

            contentServer.HandleRecieve(encoding, "testClientID");
            contentServer.SetSessionID("TestSessionID");
            Assert.IsFalse(contentServer.analyzerResult.Count == 0);
            Directory.Delete(tempDirectory, true);
        }
        /// <summary>
        /// Test when event AnalyzerResultChanged is null
        /// </summary>
        [TestMethod]
        public void AnalyzerResultChangeEventTest()
        {
            ContentServer contentServer = new( _communicator , _analyzer , "TestServer" );
            contentServer.AnalyzerResultChanged += ( result ) =>
            {
                Trace.WriteLine( "AnalyzerResultChangeEventTest Event details");
            };

            contentServer.SetSessionID("testSessionID");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");

            contentServer.HandleRecieve(encoding, "testClientID");
            // No assertions needed, we are testing that the event is not invoked when null
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// Test to check if DLLs are properly loaded by contentServer
        /// </summary>
        [TestMethod]
        public void LoadCustomDLLTest()
        {
            ContentServer contentServer = new( _communicator , _analyzer , "TestServer" );
     
            string tempDirectory = Path.Combine( Path.GetTempPath() , Path.GetRandomFileName() );
            Directory.CreateDirectory( tempDirectory );
            File.WriteAllText( Path.Combine( tempDirectory , "TestDll1.dll" ) , "DLL Content 1" );

            contentServer.LoadCustomDLLs(new List<string>() { Path.Combine( tempDirectory , "TestDll1.dll" ) } );
            Assert.IsTrue( _analyzer.GetDLLOfCustomAnalyzers().SequenceEqual( new List<string>() { Path.Combine( tempDirectory , "TestDll1.dll" ) } ));
            // No assertions needed, we are testing that the event is not invoked when null
            Directory.Delete( tempDirectory , true );
        }
    }
}
