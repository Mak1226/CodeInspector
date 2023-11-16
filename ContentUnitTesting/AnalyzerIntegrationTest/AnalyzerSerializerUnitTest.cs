/******************************************************************************
 * Filename     = AnalyzerSerializerUnitTest.cs
 * 
 * Author       = Susan
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit tests for IFileEncoder
*****************************************************************************/
using Analyzer;
using Content.Encoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentUnitTesting.AnalyzerIntegrationTest
{
    /// <summary>
    /// Class to test the IFileEncoder interface
    /// </summary>
    [TestClass]
    internal class AnalyzerSerializerUnitTest
    {
        [TestMethod]
        public void Constructor_ValidValues_PropertiesSetCorrectly()
        {
            /// Arrange
            var analyzerResult = new AnalyzerResult("Analyzer123", 1, "No errors");
            var serializer = new AnalyzerResultSerializer();

            // Act
            string serializedResult = serializer.Serialize(analyzerResult);
            var deserializedResult = serializer.Deserialize<AnalyzerResult>(serializedResult);

            // Assert
            Assert.AreEqual(analyzerResult.AnalyserID, deserializedResult.AnalyserID);
            Assert.AreEqual(analyzerResult.Verdict, deserializedResult.Verdict);
            Assert.AreEqual(analyzerResult.ErrorMessage, deserializedResult.ErrorMessage);

        }
    }
}
