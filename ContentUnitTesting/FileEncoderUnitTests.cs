﻿/******************************************************************************
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
using Content.Encoder;

namespace ContentUnitTesting
{
    /// <summary>
    /// Class to test the IFileEncoder interface
    /// </summary>
    [TestClass]
    public class FileEncoderUnitTests
    {
        private string _testDirectory;

        [TestInitialize]
        public void TestInitialize() 
        {
            _testDirectory = Directory.GetParent( Environment.CurrentDirectory ).Parent.Parent.FullName;
        }

        /// <summary>
        /// Test if a file is being encoded
        /// </summary>
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
            string encodedXML = encoder.GetEncoded( filePaths, "\\", "Test1");

            // Assert
            Assert.IsFalse( string.IsNullOrEmpty( encodedXML ) );
        }

        /// <summary>
        /// Test if a file is being encoded and then decoded properly
        /// </summary>
        [TestMethod]
        public void EncodeAndDecodeFiles_CheckEquality()
        {
            // Arrange
            DLLEncoder encoder = new();

            // Test DLL files
            var testFileNames = new List<string>
            {
                "Testdlls\\HelloWorld.dll",
                "Testdlls\\Content.dll"
            };

            var filePaths = testFileNames.Select( fileName => Path.Combine( _testDirectory , fileName ) ).ToList();

            // Save the file paths and content into a dictionary before encoding
            Dictionary<string , string> dataBeforeEncoding = new();
            foreach (string filePath in filePaths)
            {
                Trace.WriteLine( filePath );
                //Assert.IsTrue( false , filePath );
                string content = File.ReadAllText( filePath, Encoding.UTF8 );
                string relativeFilePath = Path.GetRelativePath(_testDirectory , filePath );
                dataBeforeEncoding[relativeFilePath] = content;
            }

            // Act
            string encodedXML = encoder.GetEncoded( filePaths, _testDirectory, "Test2");

            Assert.IsFalse( string.IsNullOrEmpty( encodedXML ),
                "Encoded XML is empty");

            // Decode the XML back to file paths
            encoder.DecodeFrom( encodedXML );
            Dictionary<string , string> decodedData = encoder.GetData();

            // Assert
            CollectionAssert.AreEqual(dataBeforeEncoding.Keys, decodedData.Keys);
            Assert.AreEqual(encoder.sessionID, "Test2");
            foreach (string filePath in dataBeforeEncoding.Keys)
            {
                Assert.IsTrue(decodedData.ContainsKey(filePath), $"decoded data doesn't have path {filePath}");
                Assert.AreEqual( dataBeforeEncoding[filePath] , decodedData[filePath], $"Expected : {dataBeforeEncoding[filePath]}, recieved : {decodedData[filePath]}" );
            }

        }

        [TestMethod]
        public void SaveFiles_DirectoryCorrectness()
        {
            // Arrange
            DLLEncoder encoder = new();

            // Test DLL files
            var testFileNames = new List<string>
            {
                "Testdlls\\HelloWorld.dll",
                "Testdlls\\Content.dll"
            };

            var filePaths = testFileNames.Select(fileName => Path.Combine(_testDirectory, fileName)).ToList();

            // Save the file paths before encoding

            string outputFilePath = Path.Combine(_testDirectory, "TestOutput");
            var outputFilePaths = testFileNames.Select(fileName => Path.Combine(outputFilePath, fileName)).ToList();

            // Act
            string encodedXML = encoder.GetEncoded(filePaths, _testDirectory, "Test2");

            Assert.IsFalse(string.IsNullOrEmpty(encodedXML),
                "Encoded XML is empty");

            // Decode the XML back to file paths
            encoder.DecodeFrom(encodedXML);
            Dictionary<string, string> decodedData = encoder.GetData();

            // Save files in a new directory
            encoder.SaveFiles(outputFilePath);

            // Check if each files exist
            foreach (string filePath in outputFilePaths) 
            {
                Assert.IsTrue(File.Exists(filePath), $"File {filePath} doesn't exist");
            }

            // Delete temporary files
            Directory.Delete(outputFilePath, true);
        }

    }
    
}
