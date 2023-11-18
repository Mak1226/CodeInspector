using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    public class UploadApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _sessionRoute;
        private readonly string _submissionRoute;
        private readonly string _analysisRoute;

        private const string connectionString = "UseDevelopmentStorage=true";

        public UploadApi(string sessionRoute, string submissionRoute, string analysisRoute)
        {
            _entityClient = new HttpClient();
            _sessionRoute = sessionRoute;
            _submissionRoute = submissionRoute;
            _analysisRoute = analysisRoute;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionData"></param>
        /// <returns>Returns the entity created on cloud</returns>
        public async Task<SessionEntity> PostSessionAsync(SessionData sessionData)
        {
            using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<SessionData>(_sessionRoute, sessionData);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SessionEntity entity = System.Text.Json.JsonSerializer.Deserialize<SessionEntity>(result, options);
            return entity;
        }

        public async Task<SubmissionEntity> PostSubmissionAsync(SubmissionData submissionData)
        {
            using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<SubmissionData>(_submissionRoute, submissionData);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SubmissionEntity entity = JsonSerializer.Deserialize<SubmissionEntity>(result, options);
            return entity;
        }

        public async Task<AnalysisEntity> PostAnalysisAsync(AnalysisData analysisData)
        {
            using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<AnalysisData>(_analysisRoute, analysisData);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AnalysisEntity entity = JsonSerializer.Deserialize<AnalysisEntity>(result, options);
            return entity;
        }

    }
}
