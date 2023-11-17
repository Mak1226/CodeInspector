using Microsoft.Identity.Client;
using ServerlessFunc;
using System.Text;
using System.Text.Json;

namespace UnitTests
{
    [TestClass()]
    public class InsightsTests
    {
        private string analysisUrl = "http://localhost:7074/api/analysis";
        private string submissionUrl = "http://localhost:7074/api/submission";
        private string sessionUrl = "http://localhost:7074/api/session";
        private string insightsUrl = "http://localhost:7074/api/insights";

        private DownloadApi _downloadClient;
        private UploadApi _uploadClient;
        private InsightsApi _insightsClient;

        public InsightsTests()
        {
            _downloadClient = new DownloadApi(sessionUrl, submissionUrl, analysisUrl);
            _uploadClient = new UploadApi(sessionUrl, submissionUrl, analysisUrl);
            _insightsClient = new InsightsApi(insightsUrl);
        }

        public AnalysisData GetDummyAnalysisData(string sessionId, string studentName, Dictionary<string, List<AnalyzerResult>> map)
        {
            AnalysisData analysisData = new AnalysisData();
            analysisData.SessionId = sessionId;
            analysisData.UserName = studentName;
            string json = JsonSerializer.Serialize(map);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            analysisData.AnalysisFile = byteArray;
            return analysisData;
        }
    
        public SessionData GetDummySessionData(string hostName, string sessionId, List<string> tests, List<string> students,List<Tuple<string,string>> NameToID)
        {
            SessionData sessionData = new SessionData();
            sessionData.HostUserName = hostName;
            sessionData.SessionId = sessionId;
            sessionData.Tests = InsightsUtility.ListToByte(tests);
            sessionData.Students = InsightsUtility.ListToByte(students);
            sessionData.TestNameToID = InsightsUtility.ListTupleToByte(NameToID);
            return sessionData;
        }

        public static Dictionary<string,List<AnalyzerResult>> GetAnalysisResult(int test1Verdict,int test2Verdict)
        {
            List<AnalyzerResult> result = new List<AnalyzerResult>();
            result.Add(new AnalyzerResult("101", test1Verdict, "Msg1"));
            result.Add(new AnalyzerResult("102", test2Verdict, "Msg2"));
            Dictionary<string,List<AnalyzerResult>> dictionary = new Dictionary<string,List<AnalyzerResult>>();
            dictionary["File1"] = result;
            return dictionary;
        }

        public async Task FillTestData()
        {
            List<Tuple<string, string>> NameToID = new List<Tuple<string, string>>
            {
                Tuple.Create("Test1", "101"),
                Tuple.Create("Test2", "102")
            };
            SessionData sessionData1 = GetDummySessionData("name1", "1", ["101", "102"], ["Student1","Student2"],NameToID);
            SessionData sessionData2 = GetDummySessionData("name1", "2", ["101", "102"], ["Student1", "Student2"], NameToID);
            SessionData sessionData3 = GetDummySessionData("name1", "3", ["101", "102"], ["Student1", "Student2"], NameToID);

            AnalysisData analysisData1 = GetDummyAnalysisData("1", "Student1", GetAnalysisResult(1, 0));
            AnalysisData analysisData2 = GetDummyAnalysisData("1", "Student2", GetAnalysisResult(0, 1));
            AnalysisData analysisData3 = GetDummyAnalysisData("2", "Student1", GetAnalysisResult(1, 0));
            AnalysisData analysisData4 = GetDummyAnalysisData("2", "Student2", GetAnalysisResult(1, 1));
            AnalysisData analysisData5 = GetDummyAnalysisData("3", "Student1", GetAnalysisResult(0, 0));
            AnalysisData analysisData6 = GetDummyAnalysisData("3", "Student2", GetAnalysisResult(1, 0));

            await _uploadClient.PostSessionAsync(sessionData1);
            await _uploadClient.PostSessionAsync(sessionData2);
            await _uploadClient.PostSessionAsync(sessionData3);
            await _uploadClient.PostAnalysisAsync(analysisData1);
            await _uploadClient.PostAnalysisAsync(analysisData2);
            await _uploadClient.PostAnalysisAsync(analysisData3);
            await _uploadClient.PostAnalysisAsync(analysisData4);
            await _uploadClient.PostAnalysisAsync(analysisData5);
            await _uploadClient.PostAnalysisAsync(analysisData6);

            
        }
        [TestMethod()]
        public async Task CompareTwoSessionsTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            AnalysisData analysisData1 = GetDummyAnalysisData("1", "Student1", GetAnalysisResult(1, 0));
            AnalysisData analysisData2 = GetDummyAnalysisData("1", "Student2", GetAnalysisResult(1, 1));
            AnalysisData analysisData3 = GetDummyAnalysisData("2", "Student1", GetAnalysisResult(1, 1));
            AnalysisData analysisData4 = GetDummyAnalysisData("2", "Student2", GetAnalysisResult(1, 1));
            await _uploadClient.PostAnalysisAsync(analysisData1);
            await _uploadClient.PostAnalysisAsync(analysisData2);
            await _uploadClient.PostAnalysisAsync(analysisData3);
            await _uploadClient.PostAnalysisAsync(analysisData4);

            List<Dictionary<string, int>> result = await _insightsClient.CompareTwoSessions("1", "2");
            Assert.AreEqual(result[0]["101"], 2);
            Assert.AreEqual(result[0]["102"], 1);
            Assert.AreEqual(result[1]["101"], 2);
            Assert.AreEqual(result[1]["102"], 2);
            await _downloadClient.DeleteAllAnalysisAsync();
        }

        [TestMethod()]
        public async Task GetFailedStudentsGivenTestTest()
        {
            await FillTestData();
            List<string> students = await _insightsClient.GetFailedStudentsGivenTest("name1", "102");
            students.Sort();
            List<string> expectedStudents = new List<string>
            {
                "Student1",
                "Student2"
            };
            CollectionAssert.AreEqual(expectedStudents, students);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }

        [TestMethod()]
        public async Task RunningAverageOnGivenTestTest()
        {
            await FillTestData();
            List<double> averageList = await _insightsClient.RunningAverageOnGivenTest("name1", "101");
            Assert.AreEqual(averageList[0], 50);
            Assert.AreEqual(averageList[1], 100);
            Assert.AreEqual(averageList[2], 50);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }

        [TestMethod()]
        public async Task RunningAverageOnGivenStudentTest()
        {
            await FillTestData();
            List<double> averageList = await _insightsClient.RunningAverageOnGivenStudent("name1", "Student1");
            Assert.AreEqual(averageList[0], 50);
            Assert.AreEqual(averageList[1], 50);
            Assert.AreEqual(averageList[2], 0);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }

        [TestMethod()]
        public async Task RunningAverageAcrossSessoinsTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await FillTestData();
            List<double> averageList = await _insightsClient.RunningAverageAcrossSessoins("name1");
            Assert.AreEqual(averageList[0], 50);
            Assert.AreEqual(averageList[1], 75);
            Assert.AreEqual(averageList[2], 25);

            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }

        [TestMethod()]
        public async Task StudentsWithoutAnalysisTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            List<Tuple<string, string>> NameToID = new List<Tuple<string, string>>
            {
                Tuple.Create("Test1", "101"),
                Tuple.Create("Test2", "102")
            };
            SessionData sessionData1 = GetDummySessionData("name1", "1", ["101", "102"], ["Student1", "Student2"], NameToID);
            await _uploadClient.PostSessionAsync(sessionData1);
            AnalysisData analysisData1 = GetDummyAnalysisData("1", "Student1", GetAnalysisResult(1, 0));
            await _uploadClient.PostAnalysisAsync(analysisData1);
            List<string> studentsList = await _insightsClient.UsersWithoutAnalysisGivenSession("1");
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            Assert.AreEqual(studentsList.Count, 1);
            Assert.AreEqual(studentsList[0], "Student2");
        }

        [TestMethod()]
        public async Task BestWorstAnalysisTest()
        {
            await FillTestData();

            List<string> result = await _insightsClient.GetBestWorstGivenSession("2");
            Assert.AreEqual(result.Count, 4);
            Assert.AreEqual(result[0], "Student2");
            Assert.AreEqual(result[1], "Student1");
            Assert.AreEqual(result[2], "101");
            Assert.AreEqual(result[3], "102");

            await _downloadClient.DeleteAllAnalysisAsync(); 
            await _downloadClient.DeleteAllSessionsAsync();
        }

        [TestMethod()]
        public async Task StudentScoreTest()
        {
            
            await FillTestData();
            Dictionary<string,int> StudentScore = await _insightsClient.GetStudentScoreGivenSession("1");
            Assert.AreEqual(2, StudentScore.Count);
            Assert.AreEqual(StudentScore["Student1"], 1);
            Assert.AreEqual(StudentScore["Student2"], 1);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }

        [TestMethod()]
        public async Task TestScoreTest()
        {

            await FillTestData();
            Dictionary<string, int> TestScore = await _insightsClient.GetTestScoreGivenSession("1");
            Assert.AreEqual(2, TestScore.Count);
            Assert.AreEqual(TestScore["101"], 1);
            Assert.AreEqual(TestScore["102"], 1);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }
    }
}
