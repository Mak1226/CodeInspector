/******************************************************************************
* Filename    = SessionAndAnalysisTests.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description = Testing the upload and download API by pushing and pulling from cloud
*****************************************************************************/

using System.Text;
using ServerlessFunc;

namespace CloudUnitTests
{
    /// <summary>
    /// This class contains unit tests for the Session and Analysis APIs.
    /// </summary>
    [TestClass()]
    public class SessionAndAnalysisTests
    {
        private readonly string _analysisUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/analysis";
        private readonly string _submissionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/submission";
        private readonly string _sessionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/session";
        private readonly DownloadApi _downloadClient;
        private readonly UploadApi _uploadClient;

        public SessionAndAnalysisTests()
        {
            _downloadClient = new DownloadApi( _sessionUrl , _submissionUrl , _analysisUrl );
            _uploadClient = new UploadApi( _sessionUrl , _submissionUrl , _analysisUrl );
        }

        /// <summary>
        /// Creates dummy session data with the provided parameters.
        /// </summary>
        /// <returns>A SessionData object.</returns>
        public SessionData GetDummySessionData()
        {
            SessionData sessionData = new()
            {
                HostUserName = "name1" ,
                SessionId = "1"
            };
            List<string> Test = new()
            {
                "101",
                "102"
            };
            sessionData.Tests = InsightsUtility.ListToByte( Test );
            List<string> Student = new()
            {
                "Student1",
                "Student2"
            };
            sessionData.Students = InsightsUtility.ListToByte( Student );
            List<Tuple<string , string>> NameToID = new()
            {
                Tuple.Create("Test1", "101"),
                Tuple.Create("Test2", "102")
            };
            sessionData.TestNameToID = InsightsUtility.ListTupleToByte( NameToID );
            return sessionData;
        }

        /// <summary>
        /// Tests the PostSessionAsync and GetSessionsByHostNameAsync methods.
        /// </summary>
        [TestMethod()]
        public async Task PostAndGetTestSession()
        {
            await _downloadClient.DeleteAllSessionsAsync();
            SessionData sessionData = GetDummySessionData();
            await _uploadClient.PostSessionAsync( sessionData );
            IReadOnlyList<SessionEntity> sessionEntity = await _downloadClient.GetSessionsByHostNameAsync( "name1" );
            await _downloadClient.DeleteAllSessionsAsync();
            Assert.AreEqual( 1 , sessionEntity.Count );
            CollectionAssert.AreEqual( sessionData.Students , sessionEntity[0].Students , "Students list mismatch" );
            CollectionAssert.AreEqual( sessionData.Tests , sessionEntity[0].Tests , "Tests list mismatch" );

        }

        /// <summary>
        /// Tests the PostSubmissionAsync and GetSubmissionByUserNameAndSessionIdAsync methods.
        /// </summary>
        [TestMethod()]
        public async Task PostAndGetTestSubmission()
        {
            await _downloadClient.DeleteAllSubmissionsAsync();
            SubmissionData submission = new()
            {
                SessionId = "1" ,
                UserName = "Student1" ,
                ZippedDllFiles = Encoding.ASCII.GetBytes( "demotext" )
            };
            _ = await _uploadClient.PostSubmissionAsync( submission );

            byte[] submissionFile = await _downloadClient.GetSubmissionByUserNameAndSessionIdAsync( submission.UserName , submission.SessionId );
            string text = Encoding.ASCII.GetString( submissionFile );
            await _downloadClient.DeleteAllSubmissionsAsync();
            Assert.AreEqual( text , "demotext" );

        }

        /// <summary>
        /// Tests the PostSubmissionAsync and GetSubmissionByUserNameAndSessionIdAsync methods
        /// Using a dll file
        /// </summary>
        [TestMethod()]
        public async Task PostAndGetTestSubmissionFile()
        {
            await _downloadClient.DeleteAllSubmissionsAsync();
            // Specify the path to your demo.dll file in the same directory as the test.
            string dllFilePath = Path.Combine( "..\\..\\..\\" , "demo.dll" );

            // Read the contents of the DLL file into a byte array.
            byte[] submissionFileUpload = File.ReadAllBytes( dllFilePath );

            SubmissionData submission = new()
            {
                SessionId = "1" ,
                UserName = "Student1" ,
                ZippedDllFiles = submissionFileUpload
            };

            // Call the PostSubmissionAsync method with the DLL file content.
            _ = await _uploadClient.PostSubmissionAsync( submission );

            // Call the GetSubmissionByUserNameAndSessionIdAsync to retrieve the DLL file content.
            byte[] submissionFileDownload = await _downloadClient.GetSubmissionByUserNameAndSessionIdAsync( submission.UserName , submission.SessionId );
            await _downloadClient.DeleteAllSubmissionsAsync();
            // Compare the uploaded and downloaded DLL file content.
            CollectionAssert.AreEqual( submissionFileDownload , submissionFileUpload );
        }

        /// <summary>
        /// Tests the PostAnalysisAsync and GetAnalysisByUserNameAndSessionIdAsync methods.
        /// </summary>
        [TestMethod()]
        public async Task PostAndGetTestAnalysis1()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await Task.Delay( 1000 );
            AnalysisData analysis = new()
            {
                SessionId = "1" ,
                UserName = "Student1" ,
                AnalysisFile = Encoding.ASCII.GetBytes( "demotext" )
            };
            AnalysisEntity postEntity = await _uploadClient.PostAnalysisAsync( analysis );
            await Task.Delay( 1000 );
            IReadOnlyList<AnalysisEntity> entities = await _downloadClient.GetAnalysisByUserNameAndSessionIdAsync( analysis.UserName , analysis.SessionId );
            await Task.Delay( 1000 );
            await _downloadClient.DeleteAllAnalysisAsync();
            await Task.Delay( 1000 );
            Assert.AreEqual( 1 , entities.Count );
            Assert.AreEqual( entities[0].SessionId , postEntity.SessionId );
            Assert.AreEqual( entities[0].UserName , postEntity.UserName );
            string text = Encoding.ASCII.GetString( entities[0].AnalysisFile );
            Assert.AreEqual( "demotext" , text );
        }

        /// <summary>
        /// Tests the PostAnalysisAsync and GetAnalysisBySessionIdAsync methods.
        /// </summary>
        [TestMethod()]
        public async Task PostAndGetTestAnalysis2()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            AnalysisData analysis1 = new()
            {
                SessionId = "2" ,
                UserName = "Student1" ,
                AnalysisFile = Encoding.ASCII.GetBytes( "demotext" )
            };
            AnalysisData analysis2 = new()
            {
                SessionId = "2" ,
                UserName = "Student2" ,
                AnalysisFile = Encoding.ASCII.GetBytes( "demotext" )
            };
            AnalysisEntity postEntity1 = await _uploadClient.PostAnalysisAsync( analysis1 );
            AnalysisEntity postEntity2 = await _uploadClient.PostAnalysisAsync( analysis2 );
            IReadOnlyList<AnalysisEntity> entities = await _downloadClient.GetAnalysisBySessionIdAsync("2");
            await _downloadClient.DeleteAllAnalysisAsync();
            Assert.AreEqual( 2 , entities.Count );
            Assert.AreEqual( entities[0].SessionId , postEntity1.SessionId );
            Assert.AreEqual( entities[0].UserName , postEntity1.UserName );
            Assert.AreEqual( entities[1].SessionId , postEntity2.SessionId );
            Assert.AreEqual( entities[1].UserName , postEntity2.UserName );
            string text1 = Encoding.ASCII.GetString( entities[0].AnalysisFile );
            string text2 = Encoding.ASCII.GetString( entities[1].AnalysisFile );
            Assert.AreEqual( "demotext" , text1 );
            Assert.AreEqual( "demotext" , text2 );
        }
    }
}
