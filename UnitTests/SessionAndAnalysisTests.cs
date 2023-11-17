using ServerlessFunc;
using System.Text;

namespace UnitTests
{
    [TestClass()]
    public class SessionAndAnalysisTests
    {
        private string analysisUrl = "http://localhost:7074/api/analysis";
        private string submissionUrl = "http://localhost:7074/api/submission";
        private string sessionUrl = "http://localhost:7074/api/session";
        private DownloadApi _downloadClient;
        private UploadApi _uploadClient;

        public SessionAndAnalysisTests()
        {
            _downloadClient = new DownloadApi(sessionUrl, submissionUrl, analysisUrl);
            _uploadClient = new UploadApi(sessionUrl, submissionUrl, analysisUrl);
        }

        public SessionData GetDummySessionData()
        {
            SessionData sessionData = new SessionData();
            sessionData.HostUserName = "name1";
            sessionData.SessionId = "1";
            List<string> Test = new List<string>
            {
                "101",
                "102"
            };
            sessionData.Tests = InsightsUtility.ListToByte(Test);
            List<string> Student = new List<string>
            {
                "Student1",
                "Student2"
            };
            sessionData.Students = InsightsUtility.ListToByte(Student);
            List<Tuple<string, string>> NameToID = new List<Tuple<string, string>>
            {
                Tuple.Create("Test1", "101"),
                Tuple.Create("Test2", "102")
            };
            sessionData.TestNameToID = InsightsUtility.ListTupleToByte(NameToID);
            return sessionData;
        }

        [TestMethod()]

        public async Task PostAndGetTestSession()
        {
            await _downloadClient.DeleteAllSessionsAsync();
            SessionData sessionData = GetDummySessionData();
            await _uploadClient.PostSessionAsync(sessionData);
            IReadOnlyList<SessionEntity> sessionEntity = await _downloadClient.GetSessionsByHostNameAsync("name1");
            await _downloadClient.DeleteAllSessionsAsync();
            Assert.AreEqual(1, sessionEntity.Count);
            CollectionAssert.AreEqual(sessionData.Students, sessionEntity[0].Students, "Students list mismatch");
            CollectionAssert.AreEqual(sessionData.Tests, sessionEntity[0].Tests, "Tests list mismatch");

        }
        [TestMethod()]
        public async Task PostAndGetTestSubmission()
        {

            SubmissionData submission = new SubmissionData();
            submission.SessionId = "1";
            submission.UserName = "Student1";
            submission.ZippedDllFiles = Encoding.ASCII.GetBytes("demotext");
            SubmissionEntity postEntity = await _uploadClient.PostSubmissionAsync(submission);

            byte[] submissionFile = await _downloadClient.GetSubmissionByUserNameAndSessionIdAsync(submission.UserName, submission.SessionId);
            string text = Encoding.ASCII.GetString(submissionFile);
            await _downloadClient.DeleteAllSubmissionsAsync();
            Assert.AreEqual(text, "demotext");

        }
        [TestMethod()]
        public async Task PostAndGetTestAnalysis()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            AnalysisData analysis = new AnalysisData();
            analysis.SessionId = "1";
            analysis.UserName = "Student1";
            analysis.AnalysisFile = Encoding.ASCII.GetBytes("demotext");
            AnalysisEntity postEntity = await _uploadClient.PostAnalysisAsync(analysis);
            IReadOnlyList<AnalysisEntity> entities = await _downloadClient.GetAnalysisByUserNameAndSessionIdAsync(analysis.UserName, analysis.SessionId);
            await _downloadClient.DeleteAllAnalysisAsync();
            Assert.AreEqual(1, entities.Count);
            Assert.AreEqual(entities[0].SessionId, postEntity.SessionId);
            Assert.AreEqual(entities[0].UserName, postEntity.UserName);
            string text = Encoding.ASCII.GetString(entities[0].AnalysisFile);
            Assert.AreEqual("demotext", text);

        }
    }
}
