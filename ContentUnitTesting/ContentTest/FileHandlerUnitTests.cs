/******************************************************************************
 * Filename     = FileHandlerUnitTests.cs
 * 
 * Author       = Lekshmi
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit tests for IFileHandler
*****************************************************************************/
using Content.Encoder;
using Content.FileHandling;
using System.Text.Json;

namespace ContentUnitTesting.ContentTest
{
    /// <summary>
    /// Class to test interface IFileHandler
    /// </summary>
    [TestClass]
    public class FileHandlerUnitTests
    {
        /// <summary>
        /// Test if all files in the directory are found properly
        /// </summary>
        [TestMethod]
        public void FolderFindingTest()
        {
            string tempDirectory = Path.Combine( Path.GetTempPath() , Path.GetRandomFileName() );
            Directory.CreateDirectory( tempDirectory );
            File.WriteAllText( Path.Combine( tempDirectory , "TestDll1.dll" ) , "DLL Content 1" );
            File.WriteAllText( Path.Combine( tempDirectory , "TestDll2.dll" ) , "DLL Content 2" );
            Directory.CreateDirectory( tempDirectory + "\\subdir1" );
            File.WriteAllText( Path.Combine( tempDirectory+"\\subdir1" , "TestDll3.dll" ) , "DLL Content 3" );

            IFileHandler fileHandler = new FileHandler();
            fileHandler.HandleUpload( tempDirectory , "TestSessionId" );
            List<string> filesList = fileHandler.GetFiles();
            Assert.AreEqual( filesList[0] , tempDirectory + "\\TestDll1.dll" );
            Assert.AreEqual( filesList[2] , tempDirectory + "\\subdir1" + "\\TestDll3.dll" );

            // Console.WriteLine(filesList[1] );
            // Clean up the temporary directory and files
            Directory.Delete( tempDirectory , true );
        }

        [TestMethod]
        public void FileFindingTest()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            

            IFileHandler fileHandler = new FileHandler();
            fileHandler.HandleUpload(Path.Combine(tempDirectory, "TestDll1.dll"), "TestSessionId");
            List<string> filesList = fileHandler.GetFiles();
            Assert.AreEqual(filesList[0], tempDirectory + "\\TestDll1.dll");
            // Console.WriteLine(filesList[1] );
            // Clean up the temporary directory and files
            Directory.Delete(tempDirectory, true);
        }
        [TestMethod]
        public void WrongFileTypeTest()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestTxt.txt"), "TXT Content 1");
            IFileHandler fileHandler = new FileHandler();
            fileHandler.HandleUpload(Path.Combine(tempDirectory, "TestTxt.txt"), "TestSessionId");
            List<string> filesList = fileHandler.GetFiles();
            Assert.IsTrue((filesList).Count == 0);
        }

        [TestMethod]
        public void EmptyDirectoryTest()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            IFileHandler fileHandler = new FileHandler();
            fileHandler.HandleUpload(tempDirectory, "TestSessionId");
            List<string> filesList = fileHandler.GetFiles();
            Assert.IsTrue((filesList).Count == 0);
        }

        [TestMethod]
        public void NotFileReceiveTest()
        {
            Dictionary<string, string> fileInfo = new Dictionary<string, string>();
            fileInfo["EventType"] = "NotFile";
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            Directory.CreateDirectory(tempDirectory + "\\subdir1");
            File.WriteAllText(Path.Combine(tempDirectory + "\\subdir1", "TestDll3.dll"), "DLL Content 3");

            IFileHandler fileHandler = new FileHandler();
            fileHandler.HandleUpload(tempDirectory, "TestSessionId");
            fileHandler.HandleRecieve(JsonSerializer.Serialize(fileInfo));
            Assert.IsTrue(fileHandler.GetFiles().Count() == 0);
            Directory.Delete(tempDirectory,true);
        }
        /// <summary>
        /// Test the file sending functionality by uploading files from a temporary directory
        /// and ensuring that the correct messages are sent using a file sender component.
        /// </summary>
        [TestMethod]
        public void FileSendRecieveTest()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll1.dll"), "DLL Content 1");
            File.WriteAllText(Path.Combine(tempDirectory, "TestDll2.dll"), "DLL Content 2");
            Directory.CreateDirectory(tempDirectory + "\\subdir1");
            File.WriteAllText(Path.Combine(tempDirectory + "\\subdir1", "TestDll3.dll"), "DLL Content 3");

            IFileHandler fileHandler = new FileHandler();
            string encoding = fileHandler.HandleUpload(tempDirectory, "TestSessionId");

            fileHandler.HandleRecieve(encoding);
            string receivedDirectory = Path.Combine(Environment.CurrentDirectory, "TestSessionId");

            // Check if all files in the "TestSessionId" directory have the same content as the original files
            foreach (string originalFilePath in Directory.GetFiles(tempDirectory, "*", SearchOption.AllDirectories))
            {
                string relativePath = originalFilePath.Substring(tempDirectory.Length+1);
                string newPathOfFile = Path.Combine(receivedDirectory, relativePath);
                Assert.IsTrue(File.Exists(newPathOfFile));
                if (File.Exists(newPathOfFile))
                {
                    string originalContent = File.ReadAllText(originalFilePath);
                    string newContent = File.ReadAllText(newPathOfFile);

                    Assert.AreEqual(originalContent,newContent);
                    if (originalContent != newContent)
                    {
                        Console.WriteLine($"Content mismatch for file: {relativePath}");
                    }
                }
            }

            Console.WriteLine("Content comparison completed.");

            Directory.Delete(tempDirectory, true);

        }
        [TestMethod]
        public void HandleReceive_InvalidJson_ReturnsNull()
        {
            // Arrange
            FileHandler fileHandler = new FileHandler();
            string invalidJson = "invalid json data";

            // Act
            string? result = fileHandler.HandleRecieve(invalidJson);

            // Assert
            Assert.IsNull(result, "Expected result to be null for invalid JSON");
        }

        [TestMethod]
        public void HandleReceive_WrongEventType_ReturnsNull()
        {
            // Arrange
            FileHandler fileHandler = new FileHandler();
            Dictionary<string, string> invalidData = new Dictionary<string, string>
        {
            { "EventType", "WrongEventType" },
            { "Data", "some data" }
        };
            string encoding = JsonSerializer.Serialize(invalidData);

            // Act
            string? result = fileHandler.HandleRecieve(encoding);

            // Assert
            Assert.IsNull(result, "Expected result to be null for wrong event type");
        }

    }
}

