/******************************************************************************
* Filename    = InsightsTests.cs
*
* Author      = Sahil, Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description = Testing the Insights API
*****************************************************************************/


using System.Text;
using System.Text.Json;
using ServerlessFunc;

namespace CloudUnitTests
{
    /// <summary>
    /// This class contains unit tests for the InsightsApi class.
    /// </summary>
    [TestClass()]
    public class InsightsTests
    {
        /*
        private readonly string _analysisUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/analysis";
        private readonly string _submissionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/submission";
        private readonly string _sessionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/session";
        private readonly string _insightsUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/insights";
        */
        private readonly string _analysisUrl = "http://localhost:7074/api/analysis";
        private readonly string _submissionUrl = "http://localhost:7074/api/submission";
        private readonly string _sessionUrl = "http://localhost:7074/api/session";
        private readonly string _insightsUrl = "http://localhost:7074/api/insights";
        private readonly DownloadApi _downloadClient;
        private readonly UploadApi _uploadClient;
        private readonly InsightsApi _insightsClient;

        public InsightsTests()
        {
            _downloadClient = new DownloadApi(_sessionUrl, _submissionUrl, _analysisUrl);
            _uploadClient = new UploadApi(_sessionUrl, _submissionUrl, _analysisUrl);
            _insightsClient = new InsightsApi(_insightsUrl);
        }

        /// <summary>
        /// Creates dummy analysis data for a specific session and student.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="studentName">The name of the student.</param>
        /// <param name="map">A dictionary of test names to analyzer results.</param>
        public static AnalysisData GetDummyAnalysisData(string sessionId, string studentName, Dictionary<string, List<AnalyzerResult>> map)
        {
            AnalysisData analysisData = new()
            {
                SessionId = sessionId,
                UserName = studentName
            };
            string json = JsonSerializer.Serialize(map);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            analysisData.AnalysisFile = byteArray;
            return analysisData;
        }

        /// <summary>
        /// Creates dummy session data for a specific host, session ID, tests, students, and test name to ID mapping.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="hostName">The name of the host.</param>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="tests">A list of test names.</param>
        /// <param name="students">A list of student names.</param>
        /// <param name="nameToID">A mapping of test names to test IDs.</param>
        /// <returns>The dummy session data.</returns>
        public static SessionData GetDummySessionData(string hostName, string sessionId, List<string> tests, List<string> students, List<Tuple<string, string>> NameToID)
        {
            SessionData sessionData = new()
            {
                HostUserName = hostName,
                SessionId = sessionId,
                Tests = InsightsUtility.ListToByte(tests),
                Students = InsightsUtility.ListToByte(students),
                TestNameToID = InsightsUtility.ListTupleToByte(NameToID)
            };
            return sessionData;
        }

        /// <summary>
        /// Creates dummy submission data for a specific sessionID and studentName.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="studentName">The name of the student</param>
        /// <returns>The dummy submission data.</returns>
        public static SubmissionData GetDummySubmissionData(string sessionId, string studentName)
        {
            SubmissionData submission = new()
            {
                SessionId = sessionId,
                UserName = studentName,
                ZippedDllFiles = Encoding.ASCII.GetBytes("demotext")
            };
            return submission;
        }

        /// <summary>
        /// Creates dummy analysis results for two tests.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="test1Verdict">The verdict for test 1.</param>
        /// <param name="test2Verdict">The verdict for test 2.</param>
        /// <returns>A dictionary of test names to analyzer results.</returns>
        public static Dictionary<string, List<AnalyzerResult>> GetAnalysisResult(int test1Verdict, int test2Verdict)
        {
            List<AnalyzerResult> result = new()
            {
                new AnalyzerResult( "101" , test1Verdict , "Msg1" ) ,
                new AnalyzerResult( "102" , test2Verdict , "Msg2" )
            };
            Dictionary<string, List<AnalyzerResult>> dictionary = new()
            {
                ["File1"] = result
            };
            return dictionary;
        }

        /// <summary>
        /// Fills the test data for the unit tests.
        /// </summary>
        /// <author> Sahil </author>
        public async Task FillTestData()
        {
            List<Tuple<string, string>> NameToID = new()
            {
                Tuple.Create("Test1", "101"),
                Tuple.Create("Test2", "102")
            };
            SessionData sessionData1 = GetDummySessionData("name1", "1", ["101", "102"], ["Student1", "Student2"], NameToID);
            SessionData sessionData2 = GetDummySessionData("name1", "2", ["101", "102"], ["Student1", "Student2"], NameToID);
            SessionData sessionData3 = GetDummySessionData("name1", "3", ["101", "102"], ["Student1", "Student2"], NameToID);

            SubmissionData submissionData1 = GetDummySubmissionData("1", "Student1");
            SubmissionData submissionData2 = GetDummySubmissionData("1", "Student2");
            SubmissionData submissionData3 = GetDummySubmissionData("2", "Student1");
            SubmissionData submissionData4 = GetDummySubmissionData("2", "Student2");
            SubmissionData submissionData5 = GetDummySubmissionData("3", "Student1");
            SubmissionData submissionData6 = GetDummySubmissionData("3", "Student2");

            AnalysisData analysisData1 = GetDummyAnalysisData( "1" , "Student1" , GetAnalysisResult( 1 , 0 ) );
            AnalysisData analysisData2 = GetDummyAnalysisData( "1" , "Student2" , GetAnalysisResult( 0 , 1 ) );
            AnalysisData analysisData3 = GetDummyAnalysisData( "2" , "Student1" , GetAnalysisResult( 1 , 0 ) );
            AnalysisData analysisData4 = GetDummyAnalysisData( "2" , "Student2" , GetAnalysisResult( 1 , 1 ) );
            AnalysisData analysisData5 = GetDummyAnalysisData( "3" , "Student1" , GetAnalysisResult( 0 , 0 ) );
            AnalysisData analysisData6 = GetDummyAnalysisData( "3" , "Student2" , GetAnalysisResult( 1 , 0 ) );

            await _uploadClient.PostSessionAsync( sessionData1 );
            await _uploadClient.PostSessionAsync( sessionData2 );
            await _uploadClient.PostSessionAsync( sessionData3 );
            await _uploadClient.PostAnalysisAsync( analysisData1 );
            await _uploadClient.PostAnalysisAsync( analysisData2 );
            await _uploadClient.PostAnalysisAsync( analysisData3 );
            await _uploadClient.PostAnalysisAsync( analysisData4 );
            await _uploadClient.PostAnalysisAsync( analysisData5 );
            await _uploadClient.PostAnalysisAsync( analysisData6 );
        }

        /// <summary>
        /// Tests the CompareTwoSessions method of the InsightsApi class.
        /// </summary>
        /// <author> Sahil </author>
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

        /// <summary>
        /// Tests the GetFailedStudentsGivenTest method of the InsightsApi class.
        /// </summary>
        /// <author> Sahil </author>
        [TestMethod()]
        public async Task GetFailedStudentsGivenTestTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();

            await FillTestData();
            List<string> students = await _insightsClient.GetFailedStudentsGivenTest("name1", "102");
            students.Sort();
            List<string> expectedStudents = new()
            {
                "Student1",
                "Student2"
            };
            CollectionAssert.AreEqual(expectedStudents, students);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
        }

        /// <summary>
        /// Tests the RunningAverageOnGivenTest method of the InsightsApi class.
        /// </summary>
        /// <author> Sahil </author>
        [TestMethod()]
        public async Task RunningAverageOnGivenTestTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();

            await FillTestData();
            List<double> averageList = await _insightsClient.RunningAverageOnGivenTest("name1", "101");
            Assert.AreEqual(averageList[0], 50);
            Assert.AreEqual(averageList[1], 100);
            Assert.AreEqual(averageList[2], 50);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
        }

        /// <summary>
        /// Tests the RunningAverageAcrossSessoins method of the InsightsApi class.
        /// </summary>
        /// <author> Sahil </author>
        [TestMethod()]
        public async Task RunningAverageOnGivenStudentTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();

            await FillTestData();
            List<double> averageList = await _insightsClient.RunningAverageOnGivenStudent("name1", "Student1");
            Assert.AreEqual(averageList[0], 50);
            Assert.AreEqual(averageList[1], 50);
            Assert.AreEqual(averageList[2], 0);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
        }

        /// <summary>
        /// Tests the RunningAverageAcrossSessoins method of the InsightsApi class.
        /// </summary>
        /// <author> Sahil </author>
        [TestMethod()]
        public async Task RunningAverageAcrossSessoinsTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
            await FillTestData();
            List<double> averageList = await _insightsClient.RunningAverageAcrossSessoins("name1");
            Assert.AreEqual(averageList[0], 50);
            Assert.AreEqual(averageList[1], 75);
            Assert.AreEqual(averageList[2], 25);

            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
        }

        /// <summary>
        /// Tests the UsersWithoutAnalysisGivenSession method of the InsightsApi class.
        /// </summary>
        /// <author> Nideesh N </author>
        [TestMethod()]
        public async Task StudentsWithoutAnalysisTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            List<Tuple<string , string>> NameToID = new()
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
            Assert.AreEqual( studentsList.Count , 1 );
            Assert.AreEqual( studentsList[0] , "Student2" );
        }

        /// <summary>
        /// Tests the GetBestWorstGivenSession method of the InsightsApi class.
        /// </summary>
        /// <author> Nideesh N </author>
        [TestMethod()]
        public async Task BestWorstAnalysisTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();

            await FillTestData();
            /*
            List<string> result = await _insightsClient.GetBestWorstGivenSession("2");
            Assert.AreEqual(result.Count, 4);
            Assert.AreEqual(result[0], "Student2");
            Assert.AreEqual(result[1], "Student1");
            Assert.AreEqual(result[2], "101");
            Assert.AreEqual(result[3], "102");

            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
        }

        /// <summary>
        /// Tests the GetStudentScoreGivenSession method of the InsightsApi class.
        /// </summary>
        /// <author> Nideesh N </author>
        [TestMethod()]
        public async Task StudentScoreTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
            await FillTestData();
            Dictionary<string, int> StudentScore = await _insightsClient.GetStudentScoreGivenSession("1");
            Assert.AreEqual(2, StudentScore.Count);
            Assert.AreEqual(StudentScore["Student1"], 1);
            Assert.AreEqual(StudentScore["Student2"], 1);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
        }

        /// <summary>
        /// Tests the GetTestScoreGivenSession method of the InsightsApi class.
        /// </summary>
        /// <author> Nideesh N </author>
        [TestMethod()]
        public async Task TestScoreTest()
        {
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
            await FillTestData();
            Dictionary<string, int> TestScore = await _insightsClient.GetTestScoreGivenSession("1");
            Assert.AreEqual(2, TestScore.Count);
            Assert.AreEqual(TestScore["101"], 1);
            Assert.AreEqual(TestScore["102"], 1);
            await _downloadClient.DeleteAllAnalysisAsync();
            await _downloadClient.DeleteAllSessionsAsync();
            await _downloadClient.DeleteAllSubmissionsAsync();
        }
    }
}