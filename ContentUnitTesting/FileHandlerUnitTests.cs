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

namespace ContentUnitTesting
{
    [TestClass]
    internal class FileHandlerUnitTests
    {
        [TestMethod]
        void FileFindingTest()
        {
            string tempDirectory = Path.Combine( Path.GetTempPath() , Path.GetRandomFileName() );
            Directory.CreateDirectory( tempDirectory );
            try
            {
                // Create some temporary DLL files in the test directory
                File.WriteAllText( Path.Combine( tempDirectory , "TestDll1.dll" ) , "DLL Content 1" );
                File.WriteAllText( Path.Combine( tempDirectory , "TestDll2.dll" ) , "DLL Content 2" );
                IFileHandler fileHandler = new FileHandler();
                fileHandler.Upload( tempDirectory , "TestSessionId" );
            }
            finally
            {
                // Clean up the temporary directory and files
                Directory.Delete( tempDirectory , true );
            }
        }
    }
}

