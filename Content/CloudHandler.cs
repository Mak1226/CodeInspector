using ServerlessFunc;
using Networking.Serialization;
using System.Text;
using Analyzer;

namespace Content
{
    /// <summary>
    /// Wrapper class for cloud functionality. Ideally to be done by Cloud team
    /// </summary>
    internal class CloudHandler : UploadApi
    {
        private static readonly string s_sessionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/session";
        private static readonly string s_submissionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/submission";
        private static readonly string s_analysisUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/analysis";

        private readonly string _sessionID;

        // Creates a random string as sesssion ID for this handler
        public CloudHandler() : base(s_sessionUrl, s_submissionUrl, s_analysisUrl) 
        {
            _sessionID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Creates a SessionData using given input types
        /// </summary>
        /// <param name="hostSessionID">SessionID/username of server</param>
        /// <param name="configuration">Dictionary of configuration options and whether they are being used</param>
        /// <param name="sessionList">List of sessionIDs of the sessions which are connected</param>
        /// <returns>A SessionData object</returns>
        private SessionData CreateSessionData(string hostSessionID, IDictionary<int, bool> configuration, List<string> sessionList)
        {
            SessionData sessionData = new()
            {
                HostUserName = hostSessionID ,
                SessionId = _sessionID ,
                Tests = Encoding.ASCII.GetBytes( configuration.ToString() ) ,
                TestNameToID = Encoding.ASCII.GetBytes( AnalyzerFactory.GetAllConfigurationOptions().ToString() ) ,
                Students = Encoding.ASCII.GetBytes( sessionList.ToString() )
            };

            return sessionData;
        }

        /// <summary>
        /// Creates a SubmissionData using given input types
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of DLL files</param>
        /// <returns>A SubmissionData object</returns>
        private SubmissionData CreateSubmissionData(string hostSessionID, string? encoding)
        {
            encoding ??= "";
            SubmissionData submissionData = new()
            {
                SessionId = _sessionID ,
                UserName = hostSessionID ,
                ZippedDllFiles = Encoding.ASCII.GetBytes( encoding )
            };

            return submissionData;
        }

        /// <summary>
        /// Creates a AnalysisData using given input types
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of analysis results</param>
        /// <returns>An AnalysisData object</returns>
        private AnalysisData CreateAnalysisData(string hostSessionID, string? encoding)
        {
            encoding ??= "";
            AnalysisData analysisData = new()
            {
                SessionId = _sessionID ,
                UserName = hostSessionID ,
                AnalysisFile = Encoding.ASCII.GetBytes( encoding )
            };

            return analysisData;
        }

        /// <summary>
        /// Function to send session Data to server
        /// </summary>
        /// <param name="hostSessionID">SessionID/username of server</param>
        /// <param name="configuration">Dictionary of configuration options and whether they are being used</param>
        /// <param name="sessionList">List of sessionIDs of the sessions which are connected</param>
        /// <returns></returns>
        public async Task<SessionEntity> PostSessionAsync(string hostSessionID, IDictionary<int, bool> configuration, List<string> sessionList)
        {
            return await PostSessionAsync(CreateSessionData(hostSessionID, configuration, sessionList));
        }

        /// <summary>
        /// Function to send Submission Data to server
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of DLL files</param>
        /// <returns></returns>
        public async Task<SubmissionEntity> PostSubmissionAsync(string hostSessionID, string? encoding)
        {
            return await PostSubmissionAsync(CreateSubmissionData(hostSessionID, encoding));
        }

        /// <summary>
        /// Function to send Analysis Data to server
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of analysis results</param>
        /// <returns></returns>
        public async Task<AnalysisEntity> PostAnalysisAsync(string hostSessionID, string? encoding)
        {
            return await PostAnalysisAsync(CreateAnalysisData(hostSessionID, encoding));
        }
    }
}
