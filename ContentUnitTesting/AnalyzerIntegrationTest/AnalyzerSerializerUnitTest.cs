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
using Mono.Cecil.Cil;
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
    public class AnalyzerSerializerUnitTest
    {
        private Dictionary<string, List<AnalyzerResult>> GenerateFileAnalysisDict(List<string> filePaths, List<List<List<object>>> analyzerResultDetails)
        {
            Dictionary<string, List<AnalyzerResult>> fileAnalysisDict = new Dictionary<string, List<AnalyzerResult>>();

            if (filePaths.Count != analyzerResultDetails.Count)
                throw new Exception("Invalid");

            for (int i = 0; i < filePaths.Count; i++)
            {
                string filePath = filePaths[i];
                List<List<object>> sampleList = analyzerResultDetails[i];
                List<AnalyzerResult> fileAnalysisList = new List<AnalyzerResult>();

                foreach (List<object> entry in sampleList)
                {
                    string analyserId = (string)entry[0];
                    int verdict = (int)entry[1];
                    string errorMessage = (string)entry[2];

                    // Create AnalyzerObject
                    AnalyzerResult analyzerObject = new AnalyzerResult(analyserId, verdict, errorMessage);
                    fileAnalysisList.Add(analyzerObject);
                }
                fileAnalysisDict[filePath] = fileAnalysisList;
            }

            return fileAnalysisDict;
        }

        [TestMethod]
        public void TestAnalyzerSerialization()
        {
            List<string> filePaths = new List<string> { "root/folder1/file2.dll", "root/folder2/file4.dll" };

            List<List<object>> sampleList1 = new List<List<object>>
            {
                new List<object> { "abc123", 1, "No errors" },
                new List<object> { "xyz456", 0, "Invalid input" },
                new List<object> { "123def", 2, "Internal server error" },
                new List<object> { "qwe789", 1, "File not found" }
                // Add more entries as needed
            };

            List<List<object>> sampleList2 = new List<List<object>>
            {
                new List<object> { "ghi987", 2, "Permission denied" },
                new List<object> { "jkl012", 0, "Success" },
                new List<object> { "mno345", 1, "Configuration error" },
                new List<object> { "pqr678", 2, "Timeout" }
                // Add more entries as needed
            };

            List<List<List<object>>> analyzerResultDetails = new List<List<List<object>>>();
            analyzerResultDetails.Add(sampleList1);
            analyzerResultDetails.Add(sampleList2);

            Assert.AreEqual(filePaths.Count, analyzerResultDetails.Count);

            var fileAnalysisDict = GenerateFileAnalysisDict(filePaths, analyzerResultDetails);

            AnalyzerResultSerializer analyserSerializer = new AnalyzerResultSerializer();

            // Act
            string serializedData = analyserSerializer.Serialize(fileAnalysisDict);
                Dictionary<string, List<AnalyzerResult>> deserializedResult = analyserSerializer.Deserialize<Dictionary<string, List<AnalyzerResult>>>(serializedData);
            
            
            
            // Assert
            Assert.AreEqual(fileAnalysisDict.Count, deserializedResult.Count);

            foreach (var key in fileAnalysisDict.Keys)
            {
                Assert.IsTrue(deserializedResult.ContainsKey(key), $"Key '{key}' not found in deserialized result");
                
                CollectionAssert.AreEqual(fileAnalysisDict[key], deserializedResult[key], $"Lists for key '{key}' are not equal.");
            }

        }
    }
}
