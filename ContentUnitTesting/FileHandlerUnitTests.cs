/******************************************************************************
 * Filename     = FileHandlerUnitTests.cs
 * 
 * Author       = Susan
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit tests for IFileHandler
*****************************************************************************/
using Content;
using Networking.Communicator;

namespace ContentUnitTesting
{
    /// <summary>
    /// Class to test interface IFileHandler
    /// </summary>
    [TestClass]
    public class FileHandlerUnitTests
    {
        MockCommunicator _fileSender;
        [TestInitialize]
        public void TestInitialize()
        {
            _fileSender = new MockCommunicator();
        }
        /// <summary>
        /// Test if all files in the directory are found properly
        /// </summary>
        [TestMethod]
        public void FileFindingTest()
        {
            string tempDirectory = Path.Combine( Path.GetTempPath() , Path.GetRandomFileName() );
            Directory.CreateDirectory( tempDirectory );
            File.WriteAllText( Path.Combine( tempDirectory , "TestDll1.dll" ) , "DLL Content 1" );
            File.WriteAllText( Path.Combine( tempDirectory , "TestDll2.dll" ) , "DLL Content 2" );
            Directory.CreateDirectory( tempDirectory + "\\subdir1" );
            File.WriteAllText( Path.Combine( tempDirectory+"\\subdir1" , "TestDll3.dll" ) , "DLL Content 3" );

            IFileHandler fileHandler = new FileHandler(_fileSender);
            fileHandler.Upload( tempDirectory , "TestSessionId" );
            Assert.AreEqual( fileHandler._filesList[0] , tempDirectory + "\\TestDll1.dll" );
            Assert.AreEqual( fileHandler._filesList[2] , tempDirectory + "\\subdir1" + "\\TestDll3.dll" );

            Console.WriteLine(fileHandler._filesList[1] );
            // Clean up the temporary directory and files
            Directory.Delete( tempDirectory , true );
        }
        [TestMethod]
        public void FileSendTest()
        {

            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            Directory.CreateDirectory(tempDirectory + "\\subdir1");
            File.WriteAllText(Path.Combine(tempDirectory + "\\subdir1", "TestDll3.dll"), "DLL Content 3");

            IFileHandler fileHandler = new FileHandler(_fileSender);
            int previousMessageCount = _fileSender.CheckMessageCount();
            fileHandler.Upload(tempDirectory, "TestSessionId");
            Assert.IsTrue((_fileSender.CheckMessageCount() - previousMessageCount) == 1);
            Directory.Delete(tempDirectory, true);

        }
    }
}

