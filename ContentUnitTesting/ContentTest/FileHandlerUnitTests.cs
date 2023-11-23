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
    /// Class to test file FileHandler.cs
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

        /// <summary>
        /// Test to check if given a file, processing is done properly
        /// </summary>
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

        /// <summary>
        /// Test to check if files other than dlls are properly handled
        /// </summary>
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

        /// <summary>
        /// Test to check if if directory has no dlls, its handled properly
        /// </summary>
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
        /// <summary>
        /// Test to check if eventtype associated with encoding received is matching 
        /// to that of a file. Done using sending an encoding of not file type
        /// </summary>
        [TestMethod]
        public void NotFileReceiveTest()
        {
            Dictionary<string , string> fileInfo = new()
            {
                ["EventType"] = "NotFile"
            };
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
        /// <summary>
        /// Validates that the FileHandler's HandleReceive method returns null 
        /// when provided with invalid JSON data.
        /// </summary>
        [TestMethod]
        public void InvalidJsonReceiveTest()
        {
            FileHandler fileHandler = new ();
            string invalidJson = "invalid json data";
            string? result = fileHandler.HandleRecieve(invalidJson);
            Assert.IsNull(result, "Expected result to be null for invalid JSON");
        }

    }
}

