using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    public class DownloadApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _sessionRoute;
        private readonly string _submissionRoute;
        private readonly string _analysisRoute;

        public DownloadApi(string sessionRoute, string submissionRoute, string analysisRoute)
        {
            _entityClient = new HttpClient();
            _sessionRoute = sessionRoute;
            _submissionRoute = submissionRoute;
            _analysisRoute = analysisRoute;
        }

        public async Task<IReadOnlyList<SessionEntity>> GetSessionsByHostNameAsync(string hostUsername)
        {
            var response = await _entityClient.GetAsync(_sessionRoute + $"/{hostUsername}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            IReadOnlyList<SessionEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<SessionEntity>>(result, options);
            return entities;
        }

        public async Task<byte[]> GetSubmissionByUserNameAndSessionIdAsync(string username, string sessionId)
        {
            var response = await _entityClient.GetAsync(_submissionRoute + $"/{sessionId}/{username}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            byte[] submission = JsonSerializer.Deserialize<byte[]>(result, options);
            return submission;
        }

        public async Task<IReadOnlyList<AnalysisEntity>> GetAnalysisByUserNameAndSessionIdAsync(string username, string sessionId)
        {
            var response = await _entityClient.GetAsync(_analysisRoute + $"/{sessionId}/{username}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            IReadOnlyList<AnalysisEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<AnalysisEntity>>(result, options);
            return entities;
        }
        public async Task<IReadOnlyList<AnalysisEntity>> GetAnalysisBySessionIdAsync(string sessionId)
        {
            var response = await _entityClient.GetAsync(_analysisRoute + $"/{sessionId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            IReadOnlyList<AnalysisEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<AnalysisEntity>>(result, options);
            return entities;
        }
        public async Task DeleteAllSessionsAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync(_sessionRoute);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[cloud] Network Error Exception " + ex);
            }
        }

        public async Task DeleteAllSubmissionsAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync(_submissionRoute);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[cloud] Network Error Exception " + ex);
            }
        }

        public async Task DeleteAllAnalysisAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync(_analysisRoute);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[cloud] Network Error Exception " + ex);
            }
        }
    }
}
