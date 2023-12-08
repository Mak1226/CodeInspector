/******************************************************************************
* Filename    = DownloadApi.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud
*
* Description = Provides Api functionality for the user to get files from cloud
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Logging;

namespace ServerlessFunc
{
    /// <summary>
    /// A class for interacting with the download API.
    /// </summary>
    public class DownloadApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _sessionRoute;
        private readonly string _submissionRoute;
        private readonly string _analysisRoute;

        /// <summary>
        /// Constructs a new DownloadApi instance.
        /// </summary>
        /// <param name="sessionRoute">The base URL for the session route.</param>
        /// <param name="submissionRoute">The base URL for the submission route.</param>
        /// <param name="analysisRoute">The base URL for the analysis route.</param>
        public DownloadApi( string sessionRoute , string submissionRoute , string analysisRoute )
        {
            _entityClient = new HttpClient();
            _sessionRoute = sessionRoute;
            _submissionRoute = submissionRoute;
            _analysisRoute = analysisRoute;
            Logger.Inform( "[Cloud] New download client created" );
        }

        /// <summary>
        /// Initializes a new instance of the DownloadApi class.
        /// </summary>
        /// <param name="httpClient">The HttpClient used for making HTTP requests.</param>
        /// <param name="sessionRoute">The base URL for the session endpoint.</param>
        /// <param name="submissionRoute">The base URL for the submission endpoint.</param>
        /// <param name="analysisRoute">The base URL for the analysis endpoint.</param>
        public DownloadApi( HttpClient httpClient , string sessionRoute , string submissionRoute , string analysisRoute )
        {
            _entityClient = httpClient ?? throw new ArgumentNullException( nameof( httpClient ) );
            _sessionRoute = sessionRoute;
            _submissionRoute = submissionRoute;
            _analysisRoute = analysisRoute;
            Logger.Inform( "[Cloud] New download client created" );
        }


        /// <summary>
        /// Retrieves a list of session entities for the specified host username.
        /// </summary>
        /// <param name="hostUsername">The host username to filter sessions by.</param>
        /// <returns>A collection of session entities.</returns>
        public async Task<IReadOnlyList<SessionEntity>> GetSessionsByHostNameAsync( string hostUsername )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _sessionRoute + $"/{hostUsername}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };

                IReadOnlyList<SessionEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<SessionEntity>>( result , options );
                Logger.Inform( "[Cloud] Session data by hostname GET successful" );
                return entities;
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
                return default; // or throw a custom exception, return a default value, or handle it as needed
            }
        }

        /// <summary>
        /// Retrieves the submission content associated with the specified username and session ID.
        /// </summary>
        /// <param name="username">The username of the submission.</param>
        /// <param name="sessionId">The session ID of the submission.</param>
        /// <returns>The byte array representing the submission content.</returns>
        public async Task<byte[]> GetSubmissionByUserNameAndSessionIdAsync( string username , string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _submissionRoute + $"/{sessionId}/{username}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };

                byte[] submission = JsonSerializer.Deserialize<byte[]>( result , options );
                Logger.Inform( "[Cloud] Submission data by username and sessionid GET successful" );
                return submission;
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
                return default; // or throw a custom exception, return a default value, or handle it as needed
            }
        }

        /// <summary>
        /// Retrieves the analysis results associated with the specified username and session ID.
        /// </summary>
        /// <param name="username">The username of the analysis.</param>
        /// <param name="sessionId">The session ID of the analysis.</param>
        /// <returns>A collection of AnalysisEntity objects representing the analysis results.</returns>
        public async Task<IReadOnlyList<AnalysisEntity>> GetAnalysisByUserNameAndSessionIdAsync( string username , string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _analysisRoute + $"/{sessionId}/{username}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };

                IReadOnlyList<AnalysisEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<AnalysisEntity>>( result , options );
                Logger.Inform( "[Cloud] Analysis data by username and sessionid GET successful" );
                return entities;
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
                return default; // or throw a custom exception, return a default value, or handle it as needed
            }
        }

        /// <summary>
        /// Retrieves the analysis results associated with the specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the analysis.</param>
        /// <returns>A collection of AnalysisEntity objects representing the analysis results.</returns>
        public async Task<IReadOnlyList<AnalysisEntity>> GetAnalysisBySessionIdAsync( string sessionId )
        {
            try
            {
                HttpResponseMessage response = await _entityClient.GetAsync( _analysisRoute + $"/{sessionId}" );
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true ,
                };

                IReadOnlyList<AnalysisEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<AnalysisEntity>>( result , options );
                Logger.Inform( "[Cloud] Analysis data by sessionid GET successful" );
                return entities;
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
                return default; // or throw a custom exception, return a default value, or handle it as needed
            }
        }

        /// <summary>
        /// Deletes all sessions from the database.
        /// </summary>
        public async Task DeleteAllSessionsAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync( _sessionRoute );
                response.EnsureSuccessStatusCode();
                Logger.Inform( "[Cloud] Session data DELETE successful" );
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
            }
        }

        /// <summary>
        /// Deletes all submissions from the database.
        /// </summary>
        public async Task DeleteAllSubmissionsAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync( _submissionRoute );
                response.EnsureSuccessStatusCode();
                Logger.Inform( "[Cloud] Submission data DELETE successful" );
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
            }
        }

        /// <summary>
        /// Deletes all analysis results from the database.
        /// </summary>
        public async Task DeleteAllAnalysisAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync( _analysisRoute );
                response.EnsureSuccessStatusCode();
                Logger.Inform( "[Cloud] Analysis data DELETE successful" );
            }
            catch (Exception ex)
            {
                Logger.Warn( "[cloud] Network Error Exception " + ex );
            }
        }
    }
}
