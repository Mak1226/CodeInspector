using ServerlessFunc;
using Networking.Serialization;
using System.Text;
using Analyzer;

namespace Content
{
    internal class CloudHandler : UploadApi
    {
        private static readonly string _sessionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/session";
        private static readonly string _submissionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/submission";
        private static readonly string _analysisUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/analysis";

        private readonly string _sessionID;

        public CloudHandler() : base(_sessionUrl, _submissionUrl, _analysisUrl) 
        {
            _sessionID = Guid.NewGuid().ToString();
        }

        private SessionData CreateSessionData(string hostSessionID, IDictionary<int, bool> configuration, List<string> sessionList)
        {
            SessionData sessionData = new SessionData();
            sessionData.HostUserName = hostSessionID;
            sessionData.SessionId = _sessionID;
            sessionData.Tests = Encoding.ASCII.GetBytes(configuration.ToString());
            sessionData.TestNameToID = Encoding.ASCII.GetBytes(AnalyzerFactory.GetAllConfigurationOptions().ToString());
            sessionData.Students = Encoding.ASCII.GetBytes(sessionList.ToString());

            return sessionData;
        }

        private SubmissionData CreateSubmissionData(string hostSessionID, string? encoding)
        {
            SubmissionData submissionData = new SubmissionData();
            submissionData.SessionId = _sessionID;
            submissionData.UserName = hostSessionID;
            if (encoding == null) { encoding = ""; }
            submissionData.ZippedDllFiles = Encoding.ASCII.GetBytes(encoding);

            return submissionData;
        }

        private AnalysisData CreateAnalysisData(string hostSessionID, string? encoding)
        {
            AnalysisData analysisData = new AnalysisData();
            analysisData.SessionId = _sessionID;
            analysisData.UserName = hostSessionID;
            if (encoding == null) { encoding = ""; }
            analysisData.AnalysisFile = Encoding.ASCII.GetBytes(encoding);

            return analysisData;
        }

        public async Task<SessionEntity> PostSessionAsync(string hostSessionID, IDictionary<int, bool> configuration, List<string> sessionList)
        {
            return await PostSessionAsync(CreateSessionData(hostSessionID, configuration, sessionList));
        }

        public async Task<SubmissionEntity> PostSubmissionAsync(string hostSessionID, string? encoding)
        {
            return await PostSubmissionAsync(CreateSubmissionData(hostSessionID, encoding));
        }

        public async Task<AnalysisEntity> PostAnalysisAsync(string hostSessionID, string? encoding)
        {
            return await PostAnalysisAsync(CreateAnalysisData(hostSessionID, encoding));
        }
    }
}
