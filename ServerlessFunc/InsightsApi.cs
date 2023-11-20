using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    /// <summary>
    /// This class provides methods for interacting with an Insights API.
    /// </summary>
    public class InsightsApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _insightsRoute;
        public InsightsApi( string insightsRoute )
        {
            _entityClient = new HttpClient();
            _insightsRoute = insightsRoute;
        }

        /// <summary>
        /// Compares two sessions and returns a list of dictionaries containing the comparison results.
        /// </summary>
        /// <param name="sessionId1">The ID of the first session.</param>
        /// <param name="sessionId2">The ID of the second session.</param>
        /// <returns>A list of dictionaries containing the comparison results.</returns>
        public async Task<List<Dictionary<string , int>>> CompareTwoSessions( string sessionId1 , string sessionId2 )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/compare/{sessionId1}/{sessionId2}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<Dictionary<string , int>> dictionary = JsonSerializer.Deserialize<List<Dictionary<string , int>>>( result , options );
            return dictionary;
        }

        /// <summary>
        /// Retrieves a list of students who failed a given test.
        /// </summary>
        /// <param name="hostname">The hostname of the students.</param>
        /// <param name="testName">The name of the test.</param>
        /// <returns>A list of student usernames.</returns>
        public async Task<List<string>> GetFailedStudentsGivenTest( string hostname , string testName )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/failed/{hostname}/{testName}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<string> studentList = JsonSerializer.Deserialize<List<string>>( result , options );
            return studentList;
        }

        /// <summary>
        /// Calculates the running average score for a given test across all sessions for a specified hostname.
        /// </summary>
        /// <param name="hostname">The hostname of the students.</param>
        /// <param name="testName">The name of the test.</param>
        /// <returns>A list of average scores for each session.</returns>
        public async Task<List<double>> RunningAverageOnGivenTest( string hostname , string testName )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/testaverage/{hostname}/{testName}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<double> averageList = JsonSerializer.Deserialize<List<double>>( result , options );
            return averageList;
        }

        /// <summary>
        /// Calculates the running average score for all tests across all sessions for a specified hostname and student.
        /// </summary>
        /// <param name="hostname">The hostname of the student.</param>
        /// <param name="studentName">The name of the student.</param>
        /// <returns>A list of average scores for each session.</returns>
        public async Task<List<double>> RunningAverageOnGivenStudent( string hostname , string studentName )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/studentaverage/{hostname}/{studentName}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<double> averageList = JsonSerializer.Deserialize<List<double>>( result , options );
            return averageList;
        }

        /// <summary>
        /// Calculates the average score for each session across all tests for a specified hostname.
        /// </summary>
        /// <param name="hostname">The hostname of the students.</param>
        /// <returns>A list of average scores for each session.</returns>
        public async Task<List<double>> RunningAverageAcrossSessoins( string hostname )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/sessionsaverage/{hostname}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<double> averageList = JsonSerializer.Deserialize<List<double>>( result , options );
            return averageList;
        }

        /// <summary>
        /// Retrieves a list of students who do not have an analysis report for a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A list of student usernames.</returns>
        public async Task<List<string>> UsersWithoutAnalysisGivenSession( string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/StudentsWithoutAnalysis/{sessionId}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<string> studentsList = JsonSerializer.Deserialize<List<string>>( result , options );
            return studentsList;
        }

        /// <summary>
        /// Retrieves a dictionary mapping student names to their corresponding scores for a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A dictionary mapping student names to their scores.</returns>
        public async Task<Dictionary<string , int>> GetStudentScoreGivenSession( string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/StudentScore/{sessionId}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            Dictionary<string , int> StudentScore = JsonSerializer.Deserialize<Dictionary<string , int>>( result , options );
            return StudentScore;
        }

        /// <summary>
        /// Retrieves a dictionary mapping test IDs to their corresponding average scores for a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A dictionary mapping test IDs to their average scores.</returns>
        public async Task<Dictionary<string , int>> GetTestScoreGivenSession( string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/TestScore/{sessionId}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            Dictionary<string , int> TestScore = JsonSerializer.Deserialize<Dictionary<string , int>>( result , options );
            return TestScore;
        }

        /// <summary>
        /// Retrieves a list of usernames and test IDs representing the best and worst performers in a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A list of usernames and test IDs.</returns>
        public async Task<List<string>> GetBestWorstGivenSession( string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/BestWorst/{sessionId}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,
            };
            List<string> bestworstresult = JsonSerializer.Deserialize<List<string>>( result , options );
            return bestworstresult;
        }
    }
}
