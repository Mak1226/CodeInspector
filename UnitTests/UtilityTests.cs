﻿/******************************************************************************
* Filename    = UtilityTests.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description = Testing the utility functions
*****************************************************************************/

using System.Text;
using System.Text.Json;
using ServerlessFunc;

namespace CloudUnitTests
{
    /// <summary>
    /// This class contains unit tests for the Utility classes.
    /// </summary>
    [TestClass()]
    public class UtilityTests
    {
        /// <summary>
        /// Tests the BlobUtility class.
        /// </summary>
        [TestMethod()]
        public async Task BlobUtilityTest()
        {
            string blobName = "testblob";
            byte[] blobcontent = Encoding.ASCII.GetBytes( "demotext" );
            string containerName = "democontainer";
            string connectionString = "UseDevelopmentStorage=true";
            await BlobUtility.UploadSubmissionToBlob( blobName , blobcontent , connectionString , containerName );
            byte[] getBlobContent = await BlobUtility.GetBlobContentAsync( containerName , blobName , connectionString );
            await BlobUtility.DeleteContainer( containerName , connectionString );
            CollectionAssert.AreEqual( blobcontent , getBlobContent );
        }

        /// <summary>
        /// Tests the AnalyzerResult class.
        /// </summary>
        [TestMethod()]
        public void AnalysisResultEncodeDecodeTest()
        {
            AnalyzerResult result1 = new( "100" , 1 , "Successful" );
            string jsonString1 = JsonSerializer.Serialize( result1 );
            byte[] analysisFile = Encoding.UTF8.GetBytes( jsonString1 );

            string jsonString2 = Encoding.UTF8.GetString( analysisFile );
            AnalyzerResult result2 = JsonSerializer.Deserialize<AnalyzerResult>( jsonString2 );
            Assert.AreEqual( result1.AnalyserID , result2.AnalyserID );
            Assert.AreEqual( result1.Verdict , result2.Verdict );
            Assert.AreEqual( result1.ErrorMessage , result2.ErrorMessage );
        }

        /// <summary>
        /// Tests the InsightsUtility class.
        /// </summary>
        [TestMethod()]
        public void InsightsUtilityTest1()
        {
            List<string> list = new()
            {
                "text1",
                "text2",
                "text3"
            };
            byte[] byteArray = InsightsUtility.ListToByte( list );
            List<string> deserializedList = InsightsUtility.ByteToList( byteArray );
            Assert.AreEqual( 3 , deserializedList.Count );
            for (int i = 0; i < deserializedList.Count; i++)
            {
                Assert.AreEqual( list[i] , deserializedList[i] );
            }
        }

        /// <summary>
        /// Tests the InsightsUtility class.
        /// </summary>
        [TestMethod()]
        public void InsightsUtilityTest2()
        {
            Dictionary<string , List<AnalyzerResult>> analysis = InsightsTests.GetAnalysisResult( 1 , 0 );
            byte[] analysisResultBytes = InsightsUtility.ConvertDictionaryToAnalysisFile( analysis );
            Dictionary<string , List<AnalyzerResult>> Deserializedanalysis = InsightsUtility.ConvertAnalysisFileToDictionary( analysisResultBytes );
            Assert.AreEqual( analysis.Count , Deserializedanalysis.Count );
            Assert.AreEqual( analysis["File1"][0].AnalyserID , Deserializedanalysis["File1"][0].AnalyserID );
            Assert.AreEqual( analysis["File1"][0].Verdict , Deserializedanalysis["File1"][0].Verdict );
            Assert.AreEqual( analysis["File1"][0].ErrorMessage , Deserializedanalysis["File1"][0].ErrorMessage );
        }
    }
}
