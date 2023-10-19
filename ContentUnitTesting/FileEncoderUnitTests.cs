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
using Content;

namespace ContentUnitTesting
{
    [TestClass]
    public class FileEncoderUnitTests
    {
        [TestMethod]
        public void GetEncoded_ReturnsValidXMLString()
        {
            // Arrange
            var encoder = new DLLEncoder();
            var filePaths = new List<string>
            {
                "TestDlls/HelloWorld.dll"
            };

            // Act
            string encodedXML = encoder.GetEncoded( filePaths );

            // Assert
            Assert.IsFalse( string.IsNullOrEmpty( encodedXML ) );
        }

    }
}
