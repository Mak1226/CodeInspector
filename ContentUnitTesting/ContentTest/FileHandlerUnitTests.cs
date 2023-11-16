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
using Content.FileHandling;
using ContentUnitTesting.ContentTest;
using Networking.Communicator;

namespace ContentUnitTesting
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
        public void FileFindingTest()
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
    }
}

