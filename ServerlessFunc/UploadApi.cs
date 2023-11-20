using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    /// <summary>
    /// Provides methods for interacting with an Upload API.
    /// </summary>
    public class UploadApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _sessionRoute;
        private readonly string _submissionRoute;
        private readonly string _analysisRoute;

        private const string ConnectionString = "UseDevelopmentStorage=true";

        /// <summary>
        /// Initializes a new instance of the UploadApi class.
        /// </summary>
        /// <param name="sessionRoute">The base URL for the session endpoint.</param>
        /// <param name="submissionRoute">The base URL for the submission endpoint.</param>
        /// <param name="analysisRoute">The base URL for the analysis endpoint.</param>
        public UploadApi( string sessionRoute , string submissionRoute , string analysisRoute )
        {
            _entityClient = new HttpClient();
            _sessionRoute = sessionRoute;
            _submissionRoute = submissionRoute;
            _analysisRoute = analysisRoute;
        }

        /// <summary>
        /// Creates a new session and returns the session entity.
        /// </summary>
        /// <param name="sessionData">The session data to create the new session with.</param>
        /// <returns>The newly created session entity.</returns>
        public async Task<SessionEntity> PostSessionAsync( SessionData sessionData )
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<SessionData>( _sessionRoute , sessionData );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                SessionEntity entity = System.Text.Json.JsonSerializer.Deserialize<SessionEntity>( result , options );
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine( "[UploadApi.PostSessionAsync] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Creates a new submission and returns the submission entity.
        /// </summary>
        /// <param name="submissionData">The submission data to create the new submission with.</param>
        /// <returns>The newly created submission entity.</returns>
        public async Task<SubmissionEntity> PostSubmissionAsync( SubmissionData submissionData )
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<SubmissionData>( _submissionRoute , submissionData );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                SubmissionEntity entity = JsonSerializer.Deserialize<SubmissionEntity>( result , options );
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine( "[UploadApi.PostSubmissionAsync] Exception: " + ex );
                return default;
            }
        }

        /// <summary>
        /// Creates a new analysis and returns the analysis entity.
        /// </summary>
        /// <param name="analysisData">The analysis data to create the new analysis with.</param>
        /// <returns>The newly created analysis entity.</returns>
        public async Task<AnalysisEntity> PostAnalysisAsync( AnalysisData analysisData )
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<AnalysisData>( _analysisRoute , analysisData );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                AnalysisEntity entity = JsonSerializer.Deserialize<AnalysisEntity>( result , options );
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine( "[UploadApi.PostAnalysisAsync] Exception: " + ex );
                return default;
            }
        }
    }
}
