/******************************************************************************
 * Filename    = CloudHandler.cs
 * 
 * Author      = Sahil
 *
 * Product     = Analyzer
 * 
 * Project     = Cloud
 *
 * Description = Wrapper class for handling cloud functionality
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerlessFunc;

namespace Content
{
    /// <summary>
    /// Wrapper class for cloud functionality.
    /// </summary>
    public class CloudHandler : UploadApi
    {
        /*private static readonly string s_sessionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/session";
        private static readonly string s_submissionUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/submission";
        private static readonly string s_analysisUrl = "https://serverlessfunc20231121082343.azurewebsites.net/api/analysis";*/

        private static readonly string s_sessionUrl = "http://localhost:7074/api/session";
        private static readonly string s_submissionUrl = "http://localhost:7074/api/submission";
        private static readonly string s_analysisUrl = "http://localhost:7074/api/analysis";

        private string _sessionID;

        // Creates a random string as sesssion ID for this handler
        public CloudHandler() : base( s_sessionUrl , s_submissionUrl , s_analysisUrl )
        {
            _sessionID = Guid.NewGuid().ToString();
        }

        public async Task<string> getSessionID( string hostName )
        {
            int cnt = await GetSessionCountByHostNameAsync( hostName );
            cnt += 1;
            string count = cnt.ToString();
            string[] parts = hostName.Split( "@" );
            _sessionID = parts[0] + count;
            return parts[0] + count;
        }
        /// <summary>
        /// Creates a SessionData using given input types
        /// </summary>
        /// <param name="hostSessionID">SessionID/username of server</param>
        /// <param name="configuration">Dictionary of configuration options and whether they are being used</param>
        /// <param name="sessionList">List of sessionIDs of the sessions which are connected</param>
        /// <returns>A SessionData object</returns>
        private async Task<SessionData> CreateSessionData( string hostSessionID , IDictionary<int , bool> configuration , List<string> sessionList )
        {
            List<string> tests = new();

            foreach (KeyValuePair<int , bool> kvp in configuration)
            {
                if (kvp.Value)
                {
                    tests.Add( kvp.Key.ToString() );
                }
            }
            List<Tuple<int , string>> mapping = Analyzer.AnalyzerFactory.GetAllConfigurationOptions();
            List<Tuple<string , string>> convertedMapping = mapping
            .Select( tuple => new Tuple<string , string>( tuple.Item1.ToString() , tuple.Item2 ) )
            .ToList();
            SessionData sessionData = new()
            {
                HostUserName = hostSessionID ,
                SessionId = await getSessionID( hostSessionID ) ,
                Tests = InsightsUtility.ListToByte( tests ) ,
                TestNameToID = InsightsUtility.ListTupleToByte( convertedMapping ) ,
                Students = InsightsUtility.ListToByte( sessionList )
            };

            return sessionData;
        }

        /// <summary>
        /// Creates a SubmissionData using given input types
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of DLL files</param>
        /// <returns>A SubmissionData object</returns>
        private SubmissionData CreateSubmissionData( string studentName , string? encoding )
        {
            encoding ??= "demotext";
            SubmissionData submissionData = new()
            {
                SessionId = _sessionID ,
                UserName = studentName ,
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
        private AnalysisData CreateAnalysisData( string studentName , Dictionary<string , List<Analyzer.AnalyzerResult>> data )
        {

            AnalysisData analysisData = new()
            {
                SessionId = _sessionID ,
                UserName = studentName ,
                AnalysisFile = InsightsUtility.ConvertDictionaryToAnalysisFile1( data )
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
        public async Task<SessionEntity> PostSessionAsync( string hostSessionID , IDictionary<int , bool> configuration , List<string> sessionList )
        {
            return await PostSessionAsync( await CreateSessionData( hostSessionID , configuration , sessionList ) );
        }

        /// <summary>
        /// Function to send Submission Data to server
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of DLL files</param>
        /// <returns></returns>
        public async Task<SubmissionEntity> PostSubmissionAsync( string hostSessionID , string? encoding )
        {
            return await PostSubmissionAsync( CreateSubmissionData( hostSessionID , encoding ) );
        }

        /// <summary>
        /// Function to send Analysis Data to server
        /// </summary>
        /// <param name="hostSessionID">Session ID of server</param>
        /// <param name="encoding">Encoding of analysis results</param>
        /// <returns></returns>
        public async Task<AnalysisEntity> PostAnalysisAsync( string hostSessionID , Dictionary<string , List<Analyzer.AnalyzerResult>> data )
        {
            return await PostAnalysisAsync( CreateAnalysisData( hostSessionID , data ) );
        }
    }
}
