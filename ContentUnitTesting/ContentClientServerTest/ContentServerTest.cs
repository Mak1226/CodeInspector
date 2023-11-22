using Content.Encoder;
using Content.FileHandling;
using Content.Model;

namespace ContentUnitTesting.ContentClientServerTest
{
    [TestClass]
    public class ContentServerTest
    {
        MockCommunicator _communicator;
        MockAnalyzer _analyzer;
        [TestInitialize]
        public void TestInitializer()
        {
            _communicator = new MockCommunicator();
            _analyzer = new MockAnalyzer(); 
        }
        [TestMethod]
        public void EmptyReceiveTest()
        {
            ContentServer contentServer = new ContentServer(_communicator, _analyzer);
            contentServer.HandleRecieve("","testSessionID");
            Assert.IsTrue(contentServer.analyzerResult.Count() == 0);
        }
        [TestMethod]
        public void DLLReceiveTest()
        {
            ContentServer contentServer = new ContentServer(_communicator, _analyzer);
            contentServer.SetSessionID("testSessionID");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");

            contentServer.HandleRecieve(encoding, "testClientID");
            List<string> filePaths = _analyzer.GetDllFilePath();
            List<string> expectedFilePaths = new List<string> { "testSessionID\\" + "TestDll1.dll", "testSessionID\\" + "TestDll2.dll" };
            Assert.AreEqual(filePaths[0], expectedFilePaths[0]);
            Assert.AreEqual(filePaths[1], expectedFilePaths[1]);
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// If sessionID mismatch is there, analyzerResults are not updated
        /// </summary>
        [TestMethod]
        public void SessionIDMismatchTest()
        {
            ContentServer contentServer = new ContentServer(_communicator, _analyzer);
            contentServer.SetSessionID("testSessionID1");
            IFileHandler fileHandler = new FileHandler();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            string encoding = fileHandler.HandleUpload(tempDirectory, "testSessionID");

            contentServer.HandleRecieve(encoding, "testClientID2");
            Assert.IsFalse(contentServer.analyzerResult.ContainsKey("File1"));
            Directory.Delete(tempDirectory, true);
        }

        /// <summary>
        /// If sessionID mismatch is there, analyzerResults are not updated
        /// </summary>
        [TestMethod]
        public void SessionIDMatchTest()
        {
            ContentServer contentServer = new ContentServer(_communicator, _analyzer);
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

    }
}
