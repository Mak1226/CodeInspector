/******************************************************************************
* Filename    = InsightsApi.cs
*
* Author      = Sahil
*
* Product     = Analyzer
* 
* Project     = Cloud
*
* Description = Provides Api functionality for the user to get analysis from cloud
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InsightsApi"/> class.
        /// </summary>
        /// <param name="insightsRoute">The base URL for the Insights API route.</param>
        public InsightsApi( string insightsRoute )
        {
            _entityClient = new HttpClient();
            _insightsRoute = insightsRoute;
            Trace.WriteLine( "[Cloud] New insights client created" );
        }

        /// <summary>
        /// Compares two sessions and returns a list of dictionaries containing the comparison results.
        /// </summary>
        /// <param name="sessionId1">The ID of the first session.</param>
        /// <param name="sessionId2">The ID of the second session.</param>
        /// <returns>A list of dictionaries containing the comparison results.</returns>
        public async Task<List<Dictionary<string , int>>> CompareTwoSessions( string sessionId1 , string sessionId2 )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/compare/{sessionId1}/{sessionId2}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<Dictionary<string , int>> dictionary = JsonSerializer.Deserialize<List<Dictionary<string , int>>>( result , options );
                Trace.WriteLine( $"[Cloud] CompareTwoSessions successful for {sessionId1} and {sessionId2}" );
                return dictionary;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Retrieves a list of students who failed a given test.
        /// </summary>
        /// <param name="hostname">The hostname of the students.</param>
        /// <param name="testName">The name of the test.</param>
        /// <returns>A list of student usernames.</returns>
        public async Task<List<string>> GetFailedStudentsGivenTest( string hostname , string testName )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/failed/{hostname}/{testName}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<string> studentList = JsonSerializer.Deserialize<List<string>>( result , options );
                Trace.WriteLine( $"[Cloud] GetFailedStudentsGivenTest successful for {hostname} and {testName}" );
                return studentList;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Calculates the running average score for a given test across all sessions for a specified hostname.
        /// </summary>
        /// <param name="hostname">The hostname of the students.</param>
        /// <param name="testName">The name of the test.</param>
        /// <returns>A list of average scores for each session.</returns>
        public async Task<List<double>> RunningAverageOnGivenTest( string hostname , string testName )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/testaverage/{hostname}/{testName}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<double> averageList = JsonSerializer.Deserialize<List<double>>( result , options );
                Trace.WriteLine( $"[Cloud] RunningAverageOnGivenTest successful for {hostname} and {testName}" );
                return averageList;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Calculates the running average score for all tests across all sessions for a specified hostname and student.
        /// </summary>
        /// <param name="hostname">The hostname of the student.</param>
        /// <param name="studentName">The name of the student.</param>
        /// <returns>A list of average scores for each session.</returns>
        public async Task<List<double>> RunningAverageOnGivenStudent( string hostname , string studentName )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/studentaverage/{hostname}/{studentName}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<double> averageList = JsonSerializer.Deserialize<List<double>>( result , options );
                Trace.WriteLine( $"[Cloud] RunningAverageOnGivenStudent successful for {hostname} and {studentName}" );
                return averageList;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Calculates the average score for each session across all tests for a specified hostname.
        /// </summary>
        /// <param name="hostname">The hostname of the students.</param>
        /// <returns>A list of average scores for each session.</returns>
        public async Task<List<double>> RunningAverageAcrossSessoins( string hostname )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/sessionsaverage/{hostname}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<double> averageList = JsonSerializer.Deserialize<List<double>>( result , options );
                Trace.WriteLine( $"[Cloud] RunningAverageAcrossSessoins successful for {hostname}" );
                return averageList;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Retrieves a list of students who do not have an analysis report for a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A list of student usernames.</returns>
        public async Task<List<string>> UsersWithoutAnalysisGivenSession( string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/StudentsWithoutAnalysis/{sessionId}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<string> studentsList = JsonSerializer.Deserialize<List<string>>( result , options );
                Trace.WriteLine( $"[Cloud] UsersWithoutAnalysisGivenSession successful for {sessionId}" );
                return studentsList;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi.UsersWithoutAnalysisGivenSession] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Retrieves a dictionary mapping student names to their corresponding scores for a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A dictionary mapping student names to their scores.</returns>
        public async Task<Dictionary<string , int>> GetStudentScoreGivenSession( string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/StudentScore/{sessionId}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                Dictionary<string , int> studentScore = JsonSerializer.Deserialize<Dictionary<string , int>>( result , options );
                Trace.WriteLine( $"[Cloud] GetStudentScoreGivenSession successful for {sessionId}" );
                return studentScore;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi.GetStudentScoreGivenSession] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Retrieves a dictionary mapping test IDs to their corresponding average scores for a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A dictionary mapping test IDs to their average scores.</returns>
        public async Task<Dictionary<string , int>> GetTestScoreGivenSession( string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/TestScore/{sessionId}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                Dictionary<string , int> testScore = JsonSerializer.Deserialize<Dictionary<string , int>>( result , options );
                Trace.WriteLine( $"[Cloud] GetTestScoreGivenSession successful for {sessionId}" );
                return testScore;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi.GetTestScoreGivenSession] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Retrieves a list of usernames and test IDs representing the best and worst performers in a given session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to evaluate.</param>
        /// <returns>A list of usernames and test IDs.</returns>
        public async Task<List<string>> GetBestWorstGivenSession( string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _insightsRoute + $"/BestWorst/{sessionId}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };
                List<string> bestWorstResult = JsonSerializer.Deserialize<List<string>>( result , options );
                Trace.WriteLine( $"[Cloud] GetBestWorstGivenSession successful for {sessionId}" );
                return bestWorstResult;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[InsightsApi.GetBestWorstGivenSession] Exception: " + ex );
                return default;
            }
        }
    }
}
