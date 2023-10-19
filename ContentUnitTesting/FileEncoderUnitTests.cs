/******************************************************************************
 * Filename     = FileEncoderUnitTests.cs
 * 
 * Author       = Susan
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit tests for IFileEncoder
*****************************************************************************/
using System.Diagnostics;
using System.Text;
using Content;

namespace ContentUnitTesting
{
    [TestClass]
    public class FileEncoderUnitTests
    {
        private string _testDirectory;

        [TestInitialize]
        public void TestInitialize() 
        {
            _testDirectory = Directory.GetParent( Environment.CurrentDirectory ).Parent.Parent.FullName;
        }

        [TestMethod]
        public void GetEncoded_ReturnsValidXMLString()
        {
            
            // Define the list of test file names (assuming they are already in the TestDlls directory)
            var testFileNames = new List<string>
            {
                "HelloWorld.dll"
            };

            var encoder = new DLLEncoder();

            // Create a list of file paths based on the files you've copied or created
            var filePaths = testFileNames.Select( fileName => Path.Combine( _testDirectory , fileName ) ).ToList();

            // Act
            string encodedXML = encoder.GetEncoded( filePaths );

            // Assert
            Assert.IsFalse( string.IsNullOrEmpty( encodedXML ) );
        }

        [TestMethod]
        public void EncodeAndDecodeFiles_CheckEquality()
        {
            // Arrange
            DLLEncoder encoder = new();

            // Test DLL files
            var testFileNames = new List<string>
            {
                "Testdlls/HelloWorld.dll"
            };

            var filePaths = testFileNames.Select( fileName => Path.Combine( _testDirectory , fileName ) ).ToList();

            // Save the file paths and content into a dictionary before encoding
            Dictionary<string , string> dataBeforeEncoding = new();
            foreach (string filePath in filePaths)
            {
                Trace.WriteLine( filePath );
                //Assert.IsTrue( false , filePath );
                string content = File.ReadAllText( filePath, Encoding.UTF8 );
                dataBeforeEncoding[filePath] = content;
            }

            // Act
            string encodedXML = encoder.GetEncoded( filePaths );

            Assert.IsFalse( string.IsNullOrEmpty( encodedXML ),
                "Encoded XML is empty");

            // Decode the XML back to file paths
            encoder.DecodeFrom( encodedXML );
            Dictionary<string , string> decodedData = encoder.GetData();

            // Assert
            CollectionAssert.AreEqual( dataBeforeEncoding.Keys , decodedData.Keys );
            foreach (string filePath in filePaths)
            {
                Assert.IsTrue( decodedData.ContainsKey( filePath ) );
                Assert.AreEqual( dataBeforeEncoding[filePath] , decodedData[filePath] );
            }

        }

    }
    
}
