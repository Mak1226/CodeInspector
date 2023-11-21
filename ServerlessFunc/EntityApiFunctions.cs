/******************************************************************************
* Filename    = EntityApiFunctions.cs
*
* Author      = Sahil, Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud
*
* Description = Consists of function app for all the functionalities
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServerlessFunc
{
    public static class EntityApi
    {
        private const string SessionTableName = "SessionTable";
        private const string SubmissionTableName = "SubmissionTable";
        private const string AnalysisTableName = "AnalysisTable";
        private const string ConnectionName = "AzureWebJobsStorage";
        private const string SessionRoute = "session";
        private const string SubmissionRoute = "submission";
        private const string DllContainerName = "dll";
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private const string AnalysisRoute = "analysis";
        private const string InsightsRoute = "insights";

        /// <summary>
        /// Creates a new session entity in the Azure Table storage.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="entityTable">The Azure Table storage table for storing session entities.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An HTTP response indicating the status of the session entity creation.</returns>
        [FunctionName( "CreateSessionEntity" )]
        public static async Task<IActionResult> CreateSessionEntity(
        [HttpTrigger( AuthorizationLevel.Anonymous , "post" , Route = SessionRoute )] HttpRequest req ,
        [Table( SessionTableName , Connection = ConnectionName )] IAsyncCollector<SessionEntity> entityTable ,
        ILogger log )
        {
            string requestBody = await new StreamReader( req.Body ).ReadToEndAsync();
            SessionData requestData = System.Text.Json.JsonSerializer.Deserialize<SessionData>( requestBody );
            SessionEntity value = new( requestData );
            await entityTable.AddAsync( value );
            return new OkObjectResult( value );
        }

        /// <summary>
        /// Creates a new analysis entity in the Azure Table storage.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="entityTable">The Azure Table storage table for storing analysis entities.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An HTTP response indicating the status of the analysis entity creation.</returns>
        [FunctionName( "CreateAnalysisEntity" )]
        public static async Task<IActionResult> CreateAnalysisEntity(
        [HttpTrigger( AuthorizationLevel.Anonymous , "post" , Route = AnalysisRoute )] HttpRequest req ,
        [Table( AnalysisTableName , Connection = ConnectionName )] IAsyncCollector<AnalysisEntity> entityTable ,
        ILogger log )
        {
            string requestBody = await new StreamReader( req.Body ).ReadToEndAsync();
            AnalysisData requestData = System.Text.Json.JsonSerializer.Deserialize<AnalysisData>( requestBody );
            AnalysisEntity value = new( requestData );
            await entityTable.AddAsync( value );
            return new OkObjectResult( value );
        }

        /// <summary>
        /// Creates a new submission entity in the Azure Table storage and uploads the corresponding DLL files to Azure Blob Storage.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="entityTable">The Azure Table storage table for storing submission entities.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An HTTP response indicating the status of the submission entity creation and DLL upload.</returns>
        [FunctionName( "CreateSubmissionEntity" )]
        public static async Task<IActionResult> CreateSubmissionEntity(
        [HttpTrigger( AuthorizationLevel.Anonymous , "post" , Route = SubmissionRoute )] HttpRequest req ,
        [Table( SubmissionTableName , Connection = ConnectionName )] IAsyncCollector<SubmissionEntity> entityTable ,
        ILogger log )
        {
            byte[] dllBytes;

            var streamReader = new StreamReader( req.Body );

            string requestBody = await streamReader.ReadToEndAsync();
            SubmissionData data = JsonSerializer.Deserialize<SubmissionData>( requestBody );
            dllBytes = data.ZippedDllFiles;

            await BlobUtility.UploadSubmissionToBlob( data.SessionId + '/' + data.UserName , dllBytes , ConnectionString , DllContainerName );

            SubmissionEntity value = new( data.SessionId , data.UserName );
            await entityTable.AddAsync( value );
            return new OkObjectResult( value );

        }

        /// <summary>
        /// Retrieves a list of session entities for the specified host username.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient">The Azure Table storage client for accessing the session table.</param>
        /// <param name="hostname">The host username to filter sessions by.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An HTTP response containing a list of session entities for the specified host username.</returns>
        [FunctionName( "GetSessionsbyHostname" )]
        public static async Task<IActionResult> GetSessionsbyHostname(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = SessionRoute + "/{hostname}" )] HttpRequest req ,
        [Table( SessionTableName , SessionEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient ,
        string hostname , ILogger log )
        {
            try
            {
                Page<SessionEntity> page = await tableClient.QueryAsync<SessionEntity>( filter: $"HostUserName eq '{hostname}'" ).AsPages().FirstAsync();
                return new OkObjectResult( page.Values );
            }
            catch
            {
                return new StatusCodeResult( StatusCodes.Status500InternalServerError );
            }
        }

        /// <summary>
        /// Retrieves the ZIP file containing the submitted DLL files for the specified session ID and username.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="username">The username associated with the submission.</param>
        /// <param name="sessionId">The session ID associated with the submission.</param>
        /// <returns>An HTTP response containing the ZIP file of submitted DLL files.</returns>
        [FunctionName( "GetSubmissionbyUsernameAndSessionId" )]
        public static async Task<IActionResult> GetSubmissionbyUsernameAndSessionId(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = SubmissionRoute + "/{sessionId}/{username}" )] HttpRequest req ,
        string username , string sessionId )
        {
            byte[] zippedDlls = await BlobUtility.GetBlobContentAsync( DllContainerName , sessionId + '/' + username , ConnectionString );
            return new OkObjectResult( zippedDlls );
        }

        /// <summary>
        /// Retrieves the analysis file for the specified session ID and username.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="username">The username associated with the analysis.</param>
        /// <param name="sessionId">The session ID associated with the analysis.</param>
        /// <returns>An HTTP response containing the analysis file.</returns>        [FunctionName( "GetAnalysisFilebyUsernameAndSessionId" )]
        [FunctionName( "GetAnalysisFilebyUsernameAndSessionId" )]
        public static async Task<IActionResult> GetAnalysisFilebyUsernameAndSessionId(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = AnalysisRoute + "/{sessionId}/{username}" )] HttpRequest req ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient ,
        string username , string sessionId )
        {
            Page<AnalysisEntity> page = await tableClient.QueryAsync<AnalysisEntity>( filter: $"UserName eq '{username}' and SessionId eq '{sessionId}'" ).AsPages().FirstAsync();
            return new OkObjectResult( page.Values );
        }

        /// <summary>
        /// Retrieves the analysis file for the specified session ID.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="sessionId">The session ID associated with the analysis.</param>
        /// <returns>An HTTP response containing the analysis file.</returns>
        [FunctionName( "GetAnalysisFilebySessionId" )]
        public static async Task<IActionResult> GetAnalysisFilebySessionId(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = AnalysisRoute + "/{sessionId}" )] HttpRequest req ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient ,
        string sessionId )
        {
            Page<AnalysisEntity> page = await tableClient.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionId}'" ).AsPages().FirstAsync();
            return new OkObjectResult( page.Values );
        }

        /// <summary>
        /// Removes all session entities from the Azure Table storage.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="entityClient">The Azure Table storage client for accessing the session table.</param>
        /// <returns>An HTTP response indicating the status of the session deletion.</returns>
        [FunctionName( "DeleteAllSessions" )]
        public static async Task<IActionResult> DeleteAllSessions(
        [HttpTrigger( AuthorizationLevel.Anonymous , "delete" , Route = SessionRoute )] HttpRequest req ,
        [Table( SessionTableName , ConnectionName )] TableClient entityClient )
        {
            try
            {
                await entityClient.DeleteAsync();
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }

        /// <summary>
        /// Removes all submission entities from the Azure Table storage and deletes the corresponding DLL files from Azure Blob Storage.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="entityClient">The Azure Table storage client for accessing the submission table.</param>
        /// <returns>An HTTP response indicating the status of the submission deletion.</returns>
        [FunctionName( "DeleteAllSubmissions" )]
        public static async Task<IActionResult> DeleteAllSubmissions(
        [HttpTrigger( AuthorizationLevel.Anonymous , "delete" , Route = SubmissionRoute )] HttpRequest req ,
        [Table( SubmissionTableName , ConnectionName )] TableClient entityClient )
        {
            try
            {
                await BlobUtility.DeleteContainer( DllContainerName , ConnectionString );
                await entityClient.DeleteAsync();
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }

        /// <summary>
        /// Removes all analysis entities from the Azure Table storage.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="entityClient">The Azure Table storage client for accessing the analysis table.</param>
        /// <returns>An HTTP response indicating the status of the analysis deletion.</returns>
        [FunctionName( "DeleteAllAnalysis" )]
        public static async Task<IActionResult> DeleteAllAnalysis(
        [HttpTrigger( AuthorizationLevel.Anonymous , "delete" , Route = AnalysisRoute )] HttpRequest req ,
        [Table( AnalysisTableName , ConnectionName )] TableClient entityClient )
        {
            try
            {
                await entityClient.DeleteAsync();
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }

        /// <summary>
        /// Compares two sessions based on their analysis results.
        /// </summary>
        /// <author> Sahil </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="sessionId1">The first session ID for comparison.</param>
        /// <param name="sessionId2">The second session ID for comparison.</param>
        /// <returns>An HTTP response containing a list of dictionaries representing the analysis results for both sessions.</returns>
        [FunctionName( "CompareTwoSessions" )]
        public static async Task<IActionResult> CompareTwoSessions(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/compare/{sessionId1}/{sessionId2}" )] HttpRequest req ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient ,
        string sessionId1 , string sessionId2 )
        {
            Page<AnalysisEntity> page1 = await tableClient.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionId1}'" ).AsPages().FirstAsync();
            Page<AnalysisEntity> page2 = await tableClient.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionId2}'" ).AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities1 = page1.Values.ToList();
            List<AnalysisEntity> analysisEntities2 = page2.Values.ToList();
            Dictionary<string , int> dictionary1 = new();
            Dictionary<string , int> dictionary2 = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities1)
            {
                Dictionary<string , List<AnalyzerResult>> temp = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in temp)
                {
                    foreach (AnalyzerResult analyzerResult in kvp.Value)
                    {
                        if (dictionary1.ContainsKey( analyzerResult.AnalyserID ))
                        {
                            dictionary1[analyzerResult.AnalyserID] += analyzerResult.Verdict;
                        }
                        else
                        {
                            dictionary1[analyzerResult.AnalyserID] = analyzerResult.Verdict;
                        }
                    }
                }
            }
            foreach (AnalysisEntity analysisEntity in analysisEntities2)
            {
                Dictionary<string , List<AnalyzerResult>> temp = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in temp)
                {
                    foreach (AnalyzerResult analyzerResult in kvp.Value)
                    {
                        if (dictionary2.ContainsKey( analyzerResult.AnalyserID ))
                        {
                            dictionary2[analyzerResult.AnalyserID] += analyzerResult.Verdict;
                        }
                        else
                        {
                            dictionary2[analyzerResult.AnalyserID] = analyzerResult.Verdict;
                        }
                    }
                }
            }
            List<Dictionary<string , int>> list = new()
            {
                dictionary1 ,
                dictionary2
            };
            return new OkObjectResult( list );
        }

        /// <summary>
        /// Identifies students who failed a specific test based on their analysis results.
        /// </summary>
        /// <author> Sahil </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the session table.</param>
        /// <param name="tableClient2">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="hostname">The host username to filter sessions by.</param>
        /// <param name="testid">The ID of the test to evaluate.</param>
        /// <returns>An HTTP response containing a list of usernames of students who failed the specified test.</returns>
        [FunctionName( "GetFailedStudentsGivenTest" )]
        public static async Task<IActionResult> GetFailedStudentsGivenTest(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/failed/{hostname}/{testid}" )] HttpRequest req ,
        [Table( SessionTableName , SessionEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient2 ,
        string hostname , string testid )
        {
            Page<SessionEntity> page = await tableClient1.QueryAsync<SessionEntity>( filter: $"HostUserName eq '{hostname}'" ).AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<string> studentList = new();
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                if (sessionEntity.Tests == null)
                {
                    continue;
                }
                List<string> tests = InsightsUtility.ByteToList( sessionEntity.Tests );
                if (!tests.Contains( testid ))
                {
                    continue;
                }
                Page<AnalysisEntity> page2 = await tableClient2.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionEntity.SessionId}'" ).AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                    int count = 0;
                    foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                    {
                        foreach (AnalyzerResult analyzerResult in kvp.Value)
                        {
                            if (analyzerResult.AnalyserID == testid && analyzerResult.Verdict == 0)
                            {
                                count++;
                            }
                        }
                    }
                    if (count > 0 && !studentList.Contains( analysisEntity.UserName ))
                    {
                        studentList.Add( analysisEntity.UserName );
                    }
                }
            }
            return new OkObjectResult( studentList );
        }

        /// <summary>
        /// Calculates the running average score for a specific test across multiple sessions for a given hostname.
        /// </summary>
        /// <author> Sahil </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the session table.</param>
        /// <param name="tableClient2">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="hostname">The host username to filter sessions by.</param>
        /// <param name="testid">The ID of the test to evaluate.</param>
        /// <returns>An HTTP response containing a list of average scores for each session.</returns>
        [FunctionName( "RunningAverageOnGivenTest" )]
        public static async Task<IActionResult> RunningAverageOnGivenTest(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/testaverage/{hostname}/{testid}" )] HttpRequest req ,
        [Table( SessionTableName , SessionEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient2 ,
        string hostname , string testid )
        {
            Page<SessionEntity> page = await tableClient1.QueryAsync<SessionEntity>( filter: $"HostUserName eq '{hostname}'" ).AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<double> averageList = new();
            sessionEntities.Sort( ( x , y ) => DateTime.Compare( x.Timestamp.Value.DateTime , y.Timestamp.Value.DateTime ) );
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                if (sessionEntity.Tests == null)
                {
                    continue;
                }
                List<string> tests = InsightsUtility.ByteToList( sessionEntity.Tests );
                if (!tests.Contains( testid ))
                {
                    continue;
                }
                Page<AnalysisEntity> page2 = await tableClient2.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionEntity.SessionId}'" ).AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                double sum = 0;
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                    double localSum = 0;
                    foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                    {
                        foreach (AnalyzerResult analyzerResult in kvp.Value)
                        {
                            if (analyzerResult.AnalyserID == testid)
                            {
                                localSum += analyzerResult.Verdict;
                            }
                        }
                    }
                    sum += localSum / (dictionary.Count);
                }
                if (analysisEntities.Count == 0)
                {
                    averageList.Add( 0 );
                }
                else
                {
                    averageList.Add( (sum / analysisEntities.Count) * 100 );
                }
            }

            return new OkObjectResult( averageList );
        }

        /// <summary>
        /// Calculates the running average score across multiple sessions for a given student on a specific hostname.
        /// </summary>
        /// <author> Sahil </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the session table.</param>
        /// <param name="tableClient2">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="hostname">The host username to filter sessions by.</param>
        /// <param name="studentname">The name of the student for which to calculate the running average.</param>
        /// <returns>An HTTP response containing a list of average scores for each session.</returns>
        [FunctionName( "RunningAverageOnGivenStudent" )]
        public static async Task<IActionResult> RunningAverageOnGivenStudent(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/studentaverage/{hostname}/{studentname}" )] HttpRequest req ,
        [Table( SessionTableName , SessionEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient2 ,
        string hostname , string studentname )
        {
            Page<SessionEntity> page = await tableClient1.QueryAsync<SessionEntity>( filter: $"HostUserName eq '{hostname}'" ).AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<double> averageList = new();
            sessionEntities.Sort( ( x , y ) => DateTime.Compare( x.Timestamp.Value.DateTime , y.Timestamp.Value.DateTime ) );
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                Page<AnalysisEntity> page2 = await tableClient2.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionEntity.SessionId}' and UserName eq '{studentname}'" ).AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                double sum = 0;
                int numberOfTests = 0;
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                    foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                    {
                        foreach (AnalyzerResult analyzerResult in kvp.Value)
                        {
                            sum += analyzerResult.Verdict;
                            numberOfTests++;
                        }
                    }
                }
                if (numberOfTests == 0)
                {
                    averageList.Add( 0 );
                }
                else
                {
                    averageList.Add( (sum / numberOfTests) * 100 );
                }
            }

            return new OkObjectResult( averageList );
        }

        /// <summary>
        /// Calculates the running average score across all sessions for a given hostname.
        /// </summary>
        /// <author> Sahil </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the session table.</param>
        /// <param name="tableClient2">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="hostname">The host username to filter sessions by.</param>
        /// <returns>An HTTP response containing a list of average scores for each session.</returns>
        [FunctionName( "RunningAverageAcrossSessoins" )]
        public static async Task<IActionResult> RunningAverageAcrossSessoins(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/sessionsaverage/{hostname}" )] HttpRequest req ,
        [Table( SessionTableName , SessionEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient2 ,
        string hostname )
        {
            Page<SessionEntity> page = await tableClient1.QueryAsync<SessionEntity>( filter: $"HostUserName eq '{hostname}'" ).AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<double> averageList = new();
            sessionEntities.Sort( ( x , y ) => DateTime.Compare( x.Timestamp.Value.DateTime , y.Timestamp.Value.DateTime ) );
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                Page<AnalysisEntity> page2 = await tableClient2.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionEntity.SessionId}'" ).AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                double sum = 0;
                int numberOfTests = 0;
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                    foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                    {
                        foreach (AnalyzerResult analyzerResult in kvp.Value)
                        {
                            sum += analyzerResult.Verdict;
                            numberOfTests++;
                        }
                    }
                }
                if (numberOfTests == 0)
                {
                    averageList.Add( 0 );
                }
                else
                {
                    averageList.Add( (sum / numberOfTests) * 100 );
                }
            }

            return new OkObjectResult( averageList );
        }

        /// <summary>
        /// Identifies students who do not have an analysis report for a given session.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the session table.</param>
        /// <param name="tableClient2">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="sessionid">The ID of the session to evaluate.</param>
        /// <param name="log">The logger instance for logging messages.</param>
        /// <returns>An HTTP response containing a list of usernames of students without analysis reports.</returns>
        [FunctionName( "GetUsersWithoutAnalysisGivenSession" )]
        public static async Task<IActionResult> RunningUsersWithoutAnalysis(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/StudentsWithoutAnalysis/{sessionid}" )] HttpRequest req ,
        [Table( SessionTableName , SessionEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient2 ,
        string sessionid ,
        ILogger log
            )
        {
            Page<SessionEntity> page = await tableClient1.QueryAsync<SessionEntity>( filter: $"SessionId eq '{sessionid}'" ).AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            SessionEntity sessionEntity = sessionEntities[0];
            List<string> students = InsightsUtility.ByteToList( sessionEntity.Students );
            Page<AnalysisEntity> page2 = await tableClient2.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionid}'" ).AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page2.Values.ToList();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                students.Remove( analysisEntity.UserName );
            }
            return new OkObjectResult( students );
        }

        /// <summary>
        /// Identifies the student with the highest and lowest overall score, 
        /// and the test with the highest and lowest average score for a given session.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="sessionid">The ID of the session to evaluate.</param>
        /// <param name="log">The logger instance for logging messages.</param>
        /// <returns>An HTTP response containing a list of usernames and test IDs representing the best and worst performers.</returns>
        [FunctionName( "GetBestWorstGivenSession" )]
        public static async Task<IActionResult> RunningBestWorst(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/BestWorst/{sessionid}" )] HttpRequest req ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        string sessionid ,
        ILogger log
            )
        {
            Page<AnalysisEntity> page = await tableClient1.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionid}'" ).AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page.Values.ToList();
            List<string> result = new();
            Dictionary<string , int> StudentScore = new();
            Dictionary<string , int> TestScore = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                {
                    foreach (AnalyzerResult analyserResult in kvp.Value)
                    {
                        if (!StudentScore.ContainsKey( analysisEntity.UserName ))
                        {
                            StudentScore[analysisEntity.UserName] = 0;
                        }
                        StudentScore[analysisEntity.UserName] += analyserResult.Verdict;
                        if (!TestScore.ContainsKey( analyserResult.AnalyserID ))
                        {
                            TestScore[analyserResult.AnalyserID] = 0;
                        }
                        TestScore[analyserResult.AnalyserID] += analyserResult.Verdict;
                    }
                }
            }
            KeyValuePair<string , int> studentWithHighestScore = StudentScore.Aggregate( ( x , y ) => x.Value > y.Value ? x : y );
            KeyValuePair<string , int> studentWithLowestScore = StudentScore.Aggregate( ( x , y ) => x.Value < y.Value ? x : y );
            KeyValuePair<string , int> testtWithHighestScore = TestScore.Aggregate( ( x , y ) => x.Value > y.Value ? x : y );
            KeyValuePair<string , int> testtWithLowestScore = TestScore.Aggregate( ( x , y ) => x.Value < y.Value ? x : y );
            result.Add( studentWithHighestScore.Key );
            result.Add( studentWithLowestScore.Key );
            result.Add( testtWithHighestScore.Key );
            result.Add( testtWithLowestScore.Key );
            return new OkObjectResult( result );
        }

        /// <summary>
        /// Retrieves the score for each student in a given session.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="sessionid">The ID of the session to evaluate.</param>
        /// <param name="log">The logger instance for logging messages.</param>
        /// <returns>An HTTP response containing a dictionary mapping student names to their corresponding scores.</returns>
        [FunctionName( "GetStudentScoreGivenSession" )]
        public static async Task<IActionResult> RunningStudentScore(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/StudentScore/{sessionid}" )] HttpRequest req ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        string sessionid ,
        ILogger log
            )
        {
            Page<AnalysisEntity> page = await tableClient1.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionid}'" ).AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page.Values.ToList();
            _ = new List<string>();
            Dictionary<string , int> StudentScore = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                {
                    foreach (AnalyzerResult analyserResult in kvp.Value)
                    {
                        if (!StudentScore.ContainsKey( analysisEntity.UserName ))
                        {
                            StudentScore[analysisEntity.UserName] = 0;
                        }
                        StudentScore[analysisEntity.UserName] += analyserResult.Verdict;
                    }
                }

            }
            return new OkObjectResult( StudentScore );
        }

        /// <summary>
        /// Retrieves the average score for each test in a given session.
        /// </summary>
        /// <author> Nideesh N </author>
        /// <param name="req">The HTTP request object.</param>
        /// <param name="tableClient1">The Azure Table storage client for accessing the analysis table.</param>
        /// <param name="sessionid">The ID of the session to evaluate.</param>
        /// <param name="log">The logger instance for logging messages.</param>
        /// <returns>An HTTP response containing a dictionary mapping test IDs to their corresponding average scores.</returns>
        [FunctionName( "GetTestScoreGivenSession" )]
        public static async Task<IActionResult> RunningTestScore(
        [HttpTrigger( AuthorizationLevel.Anonymous , "get" , Route = InsightsRoute + "/TestScore/{sessionid}" )] HttpRequest req ,
        [Table( AnalysisTableName , AnalysisEntity.PartitionKeyName , Connection = ConnectionName )] TableClient tableClient1 ,
        string sessionid ,
        ILogger log
            )
        {
            Page<AnalysisEntity> page = await tableClient1.QueryAsync<AnalysisEntity>( filter: $"SessionId eq '{sessionid}'" ).AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page.Values.ToList();
            _ = new List<string>();
            Dictionary<string , int> TestScore = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                Dictionary<string , List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary( analysisEntity.AnalysisFile );
                foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                {
                    foreach (AnalyzerResult analyserResult in kvp.Value)
                    {
                        if (!TestScore.ContainsKey( analyserResult.AnalyserID ))
                        {
                            TestScore[analyserResult.AnalyserID] = 0;
                        }
                        TestScore[analyserResult.AnalyserID] += analyserResult.Verdict;
                    }
                }

            }
            return new OkObjectResult( TestScore );
        }
    }
}
