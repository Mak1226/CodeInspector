using System.Text;
using ServerlessFunc;

namespace UnitTests
{
    /// <summary>
    /// This class contains unit tests for the Session and Analysis APIs.
    /// </summary>
    [TestClass()]
    public class SessionAndAnalysisTests
    {
        private readonly string _analysisUrl = "http://localhost:7074/api/analysis";
        private readonly string _submissionUrl = "http://localhost:7074/api/submission";
        private readonly string _sessionUrl = "http://localhost:7074/api/session";
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
        /// Tests the PostAnalysisAsync and GetAnalysisByUserNameAndSessionIdAsync methods.
        /// </summary>
        [TestMethod()]
        public async Task PostAndGetTestAnalysis()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            AnalysisData analysis = new()
            {
                SessionId = "1" ,
                UserName = "Student1" ,
                AnalysisFile = Encoding.ASCII.GetBytes( "demotext" )
            };
            AnalysisEntity postEntity = await _uploadClient.PostAnalysisAsync( analysis );
            IReadOnlyList<AnalysisEntity> entities = await _downloadClient.GetAnalysisByUserNameAndSessionIdAsync( analysis.UserName , analysis.SessionId );
            await _downloadClient.DeleteAllAnalysisAsync();
            Assert.AreEqual( 1 , entities.Count );
            Assert.AreEqual( entities[0].SessionId , postEntity.SessionId );
            Assert.AreEqual( entities[0].UserName , postEntity.UserName );
            string text = Encoding.ASCII.GetString( entities[0].AnalysisFile );
            Assert.AreEqual( "demotext" , text );
        }
    }
}
