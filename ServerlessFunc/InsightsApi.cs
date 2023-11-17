using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    public class InsightsApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _insightsRoute;
        public InsightsApi(string insightsRoute)
        {
            _entityClient = new HttpClient();
            _insightsRoute = insightsRoute;
        }

        public async Task<List<Dictionary<string, int>>> CompareTwoSessions(string sessionId1, string sessionId2)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/compare/{sessionId1}/{sessionId2}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Dictionary<string, int>> dictionary = JsonSerializer.Deserialize<List<Dictionary<string, int>>>(result, options);
            return dictionary;
        }

        public async Task<List<string>> GetFailedStudentsGivenTest(string hostname, string testName)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/failed/{hostname}/{testName}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<string> studentList = JsonSerializer.Deserialize<List<string>>(result, options);
            return studentList;
        }

        public async Task<List<double>> RunningAverageOnGivenTest(string hostname, string testName)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/testaverage/{hostname}/{testName}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<double> averageList = JsonSerializer.Deserialize<List<double>>(result, options);
            return averageList;
        }

        public async Task<List<double>> RunningAverageOnGivenStudent(string hostname, string studentName)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/studentaverage/{hostname}/{studentName}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<double> averageList = JsonSerializer.Deserialize<List<double>>(result, options);
            return averageList;
        }
        public async Task<List<double>> RunningAverageAcrossSessoins(string hostname)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/sessionsaverage/{hostname}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<double> averageList = JsonSerializer.Deserialize<List<double>>(result, options);
            return averageList;
        }

        public async Task<List<string>> UsersWithoutAnalysisGivenSession(string sessionId)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/StudentsWithoutAnalysis/{sessionId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<string> studentsList = JsonSerializer.Deserialize<List<string>>(result, options);
            return studentsList;
        }

        public async Task<Dictionary<string, int>> GetStudentScoreGivenSession(string sessionId)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/StudentScore/{sessionId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Dictionary<string,int> StudentScore = JsonSerializer.Deserialize<Dictionary<string,int>>(result, options);
            return StudentScore;
        }

        public async Task<Dictionary<string, int>> GetTestScoreGivenSession(string sessionId)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/TestScore/{sessionId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Dictionary<string, int> TestScore = JsonSerializer.Deserialize<Dictionary<string, int>>(result, options);
            return TestScore;
        }

        public async Task<List<string>> GetBestWorstGivenSession(string sessionId)
        {
            var response = await _entityClient.GetAsync(_insightsRoute + $"/BestWorst/{sessionId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<string> bestworstresult = JsonSerializer.Deserialize<List<string>>(result, options);
            return bestworstresult;
        }
    }
}
