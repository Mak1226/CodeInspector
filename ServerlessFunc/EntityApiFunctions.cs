

using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private const string connectionString = "UseDevelopmentStorage=true";
        private const string AnalysisRoute = "analysis";
        private const string InsightsRoute = "insights";

        [FunctionName("CreateSessionEntity")]
        public static async Task<IActionResult> CreateSessionEntity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = SessionRoute)] HttpRequest req,
        [Table(SessionTableName, Connection = ConnectionName)] IAsyncCollector<SessionEntity> entityTable,
        ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SessionData requestData = System.Text.Json.JsonSerializer.Deserialize<SessionData>(requestBody);
            SessionEntity value = new SessionEntity(requestData);
            await entityTable.AddAsync(value);
            return new OkObjectResult(value);
        }

        [FunctionName("CreateAnalysisEntity")]
        public static async Task<IActionResult> CreateAnalysisEntity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = AnalysisRoute)] HttpRequest req,
        [Table(AnalysisTableName, Connection = ConnectionName)] IAsyncCollector<AnalysisEntity> entityTable,
        ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AnalysisData requestData = System.Text.Json.JsonSerializer.Deserialize<AnalysisData>(requestBody);
            AnalysisEntity value = new AnalysisEntity(requestData);
            await entityTable.AddAsync(value);
            return new OkObjectResult(value);
        }

        [FunctionName("CreateSubmissionEntity")]
        public static async Task<IActionResult> CreateSubmissionEntity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = SubmissionRoute)] HttpRequest req,
        [Table(SubmissionTableName, Connection = ConnectionName)] IAsyncCollector<SubmissionEntity> entityTable,
        ILogger log)
        {
            byte[] dllBytes;

            var streamReader = new StreamReader(req.Body);

            var requestBody = await streamReader.ReadToEndAsync();
            SubmissionData data = JsonSerializer.Deserialize<SubmissionData>(requestBody);
            dllBytes = data.ZippedDllFiles;

            await BlobUtility.UploadSubmissionToBlob(data.SessionId + '/' + data.UserName, dllBytes, connectionString, DllContainerName);

            SubmissionEntity value = new SubmissionEntity(data.SessionId, data.UserName);
            await entityTable.AddAsync(value);
            return new OkObjectResult(value);

        }


        [FunctionName("GetSessionsbyHostname")]
        public static async Task<IActionResult> GetSessionsbyHostname(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = SessionRoute + "/{hostname}")] HttpRequest req,
        [Table(SessionTableName, SessionEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient,
        string hostname, ILogger log)
        {
            try
            {
                var page = await tableClient.QueryAsync<SessionEntity>(filter: $"HostUserName eq '{hostname}'").AsPages().FirstAsync();
                return new OkObjectResult(page.Values);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        [FunctionName("GetSubmissionbyUsernameAndSessionId")]
        public static async Task<IActionResult> GetSubmissionbyUsernameAndSessionId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = SubmissionRoute + "/{sessionId}/{username}")] HttpRequest req,
        string username, string sessionId)
        {
            byte[] zippedDlls = await BlobUtility.GetBlobContentAsync(DllContainerName, sessionId + '/' + username, connectionString);
            return new OkObjectResult(zippedDlls);
        }

        [FunctionName("GetAnalysisFilebyUsernameAndSessionId")]
        public static async Task<IActionResult> GetAnalysisFilebyUsernameAndSessionId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = AnalysisRoute + "/{sessionId}/{username}")] HttpRequest req,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient,
        string username, string sessionId)
        {
            var page = await tableClient.QueryAsync<AnalysisEntity>(filter: $"UserName eq '{username}' and SessionId eq '{sessionId}'").AsPages().FirstAsync();
            return new OkObjectResult(page.Values);
        }

        [FunctionName("GetAnalysisFilebySessionId")]
        public static async Task<IActionResult> GetAnalysisFilebySessionId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = AnalysisRoute + "/{sessionId}")] HttpRequest req,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient,
        string sessionId)
        {
            var page = await tableClient.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionId}'").AsPages().FirstAsync();
            return new OkObjectResult(page.Values);
        }

        [FunctionName("DeleteAllSessions")]
        public static async Task<IActionResult> DeleteAllSessions(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = SessionRoute)] HttpRequest req,
        [Table(SessionTableName, ConnectionName)] TableClient entityClient)
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

        [FunctionName("DeleteAllSubmissions")]
        public static async Task<IActionResult> DeleteAllSubmissions(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = SubmissionRoute)] HttpRequest req,
        [Table(SubmissionTableName, ConnectionName)] TableClient entityClient)
        {
            try
            {
                await BlobUtility.DeleteContainer(DllContainerName, connectionString);
                await entityClient.DeleteAsync();
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }

        [FunctionName("DeleteAllAnalysis")]
        public static async Task<IActionResult> DeleteAllAnalysis(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = AnalysisRoute)] HttpRequest req,
        [Table(AnalysisTableName, ConnectionName)] TableClient entityClient)
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

        [FunctionName("CompareTwoSessions")]
        public static async Task<IActionResult> CompareTwoSessions(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/compare/{sessionId1}/{sessionId2}")] HttpRequest req,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient,
        string sessionId1, string sessionId2)
        {
            var page1 = await tableClient.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionId1}'").AsPages().FirstAsync();
            var page2 = await tableClient.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionId2}'").AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities1 = page1.Values.ToList();
            List<AnalysisEntity> analysisEntities2 = page2.Values.ToList();
            Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            foreach (AnalysisEntity analysisEntity in analysisEntities1)
            {
                Dictionary<string, List<AnalyzerResult>> temp = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                foreach (var kvp in temp)
                {
                    foreach (var analyzerResult in kvp.Value)
                    {
                        if (dictionary1.ContainsKey(analyzerResult.AnalyserID))
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
                Dictionary<string, List<AnalyzerResult>> temp = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                foreach (var kvp in temp)
                {
                    foreach (var analyzerResult in kvp.Value)
                    {
                        if (dictionary2.ContainsKey(analyzerResult.AnalyserID))
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
            List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
            list.Add(dictionary1);
            list.Add(dictionary2);
            return new OkObjectResult(list);
        }

        [FunctionName("GetFailedStudentsGivenTest")]
        public static async Task<IActionResult> GetFailedStudentsGivenTest(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/failed/{hostname}/{testid}")] HttpRequest req,
        [Table(SessionTableName, SessionEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient2,
        string hostname, string testid)
        {
            var page = await tableClient1.QueryAsync<SessionEntity>(filter: $"HostUserName eq '{hostname}'").AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<string> studentList = new List<string>();
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                if (sessionEntity.Tests == null)
                {
                    continue;
                }
                List<string> tests = InsightsUtility.ByteToList(sessionEntity.Tests);
                if (!tests.Contains(testid))
                {
                    continue;
                }
                var page2 = await tableClient2.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionEntity.SessionId}'").AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                    int count = 0;
                    foreach(var kvp in dictionary)
                    {
                        foreach (var analyzerResult in kvp.Value)
                        {
                            if(analyzerResult.AnalyserID == testid && analyzerResult.Verdict == 0)
                            {
                                count++;
                            }
                        }
                    }
                    if (count > 0 && !studentList.Contains(analysisEntity.UserName))
                    {
                        studentList.Add(analysisEntity.UserName);
                    }
                }
            }
            return new OkObjectResult(studentList);
        }
        [FunctionName("RunningAverageOnGivenTest")]
        public static async Task<IActionResult> RunningAverageOnGivenTest(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/testaverage/{hostname}/{testid}")] HttpRequest req,
        [Table(SessionTableName, SessionEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient2,
        string hostname, string testid)
        {
            var page = await tableClient1.QueryAsync<SessionEntity>(filter: $"HostUserName eq '{hostname}'").AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<double> averageList = new List<double>();
            sessionEntities.Sort((x, y) => DateTime.Compare(x.Timestamp.Value.DateTime, y.Timestamp.Value.DateTime));
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                if (sessionEntity.Tests == null)
                {
                    continue;
                }
                List<string> tests = InsightsUtility.ByteToList(sessionEntity.Tests);
                if (!tests.Contains(testid))
                {
                    continue;
                }
                var page2 = await tableClient2.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionEntity.SessionId}'").AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                double sum = 0;
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                    double localSum = 0;
                    foreach(var kvp in dictionary)
                    {
                        foreach(var analyzerResult in kvp.Value)
                        {
                            if(analyzerResult.AnalyserID == testid)
                            {
                                localSum += analyzerResult.Verdict;
                            }
                        }
                    }
                    sum += localSum / (dictionary.Count);
                }
                if (analysisEntities.Count == 0)
                {
                    averageList.Add(0);
                }
                else
                {
                    averageList.Add((sum / analysisEntities.Count) * 100);
                }
            }

            return new OkObjectResult(averageList);
        }

        [FunctionName("RunningAverageOnGivenStudent")]
        public static async Task<IActionResult> RunningAverageOnGivenStudent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/studentaverage/{hostname}/{studentname}")] HttpRequest req,
        [Table(SessionTableName, SessionEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient2,
        string hostname, string studentname)
        {
            var page = await tableClient1.QueryAsync<SessionEntity>(filter: $"HostUserName eq '{hostname}'").AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<double> averageList = new List<double>();
            sessionEntities.Sort((x, y) => DateTime.Compare(x.Timestamp.Value.DateTime, y.Timestamp.Value.DateTime));
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                var page2 = await tableClient2.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionEntity.SessionId}' and UserName eq '{studentname}'").AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                double sum = 0;
                int numberOfTests = 0;
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                    foreach (var kvp in dictionary)
                    {
                        foreach(var analyzerResult in kvp.Value)
                        {
                            sum += analyzerResult.Verdict;
                            numberOfTests++;
                        }
                        
                    }
                }
                if (numberOfTests == 0)
                {
                    averageList.Add(0);
                }
                else
                {
                    averageList.Add((sum / numberOfTests) * 100);
                }
            }

            return new OkObjectResult(averageList);
        }

        [FunctionName("RunningAverageAcrossSessoins")]
        public static async Task<IActionResult> RunningAverageAcrossSessoins(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/sessionsaverage/{hostname}")] HttpRequest req,
        [Table(SessionTableName, SessionEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient2,
        string hostname)
        {
            var page = await tableClient1.QueryAsync<SessionEntity>(filter: $"HostUserName eq '{hostname}'").AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            List<double> averageList = new List<double>();
            sessionEntities.Sort((x, y) => DateTime.Compare(x.Timestamp.Value.DateTime, y.Timestamp.Value.DateTime));
            foreach (SessionEntity sessionEntity in sessionEntities)
            {
                var page2 = await tableClient2.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionEntity.SessionId}'").AsPages().FirstAsync();
                List<AnalysisEntity> analysisEntities = page2.Values.ToList();
                double sum = 0;
                int numberOfTests = 0;
                foreach (AnalysisEntity analysisEntity in analysisEntities)
                {
                    Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                    foreach (var kvp in dictionary)
                    {
                        foreach(var analyzerResult in kvp.Value)
                        {
                            sum += analyzerResult.Verdict;
                            numberOfTests++;
                        }
                    }
                }
                if (numberOfTests == 0)
                {
                    averageList.Add(0);
                }
                else
                {
                    averageList.Add((sum / numberOfTests) * 100);
                }
            }

            return new OkObjectResult(averageList);
        }

        [FunctionName("GetUsersWithoutAnalysisGivenSession")]
        public static async Task<IActionResult> RunningUsersWithoutAnalysis(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/StudentsWithoutAnalysis/{sessionid}")] HttpRequest req,
        [Table(SessionTableName, SessionEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient2,
        string sessionid,
        ILogger log
            )
        {
            var page = await tableClient1.QueryAsync<SessionEntity>(filter: $"SessionId eq '{sessionid}'").AsPages().FirstAsync();
            List<SessionEntity> sessionEntities = page.Values.ToList();
            SessionEntity sessionEntity = sessionEntities[0];
            List<string> students = InsightsUtility.ByteToList(sessionEntity.Students);
            var page2 = await tableClient2.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionid}'").AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page2.Values.ToList();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                students.Remove(analysisEntity.UserName);
            }
            return new OkObjectResult(students);
        }

        [FunctionName("GetBestWorstGivenSession")]
        public static async Task<IActionResult> RunningBestWorst(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/BestWorst/{sessionid}")] HttpRequest req,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        string sessionid,
        ILogger log
            )
        {
            var page = await tableClient1.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionid}'").AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page.Values.ToList();
            List<string> result = new();
            Dictionary<string, int> StudentScore = new();
            Dictionary<string, int> TestScore = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                foreach(var kvp in dictionary)
                {
                    foreach(var analyserResult in kvp.Value)
                    {
                        if (!StudentScore.ContainsKey(analysisEntity.UserName))
                        {
                            StudentScore[analysisEntity.UserName] = 0;
                        }
                        StudentScore[analysisEntity.UserName] += analyserResult.Verdict;
                        if (!TestScore.ContainsKey(analyserResult.AnalyserID))
                        {
                            TestScore[analyserResult.AnalyserID] = 0;
                        }
                        TestScore[analyserResult.AnalyserID] += analyserResult.Verdict;
                    }
                }
                
            }
            var studentWithHighestScore = StudentScore.Aggregate((x, y) => x.Value > y.Value ? x : y);
            var studentWithLowestScore = StudentScore.Aggregate((x, y) => x.Value < y.Value ? x : y);
            var testtWithHighestScore = TestScore.Aggregate((x, y) => x.Value > y.Value ? x : y);
            var testtWithLowestScore = TestScore.Aggregate((x, y) => x.Value < y.Value ? x : y);
            result.Add(studentWithHighestScore.Key);
            result.Add(studentWithLowestScore.Key);
            result.Add(testtWithHighestScore.Key);
            result.Add(testtWithLowestScore.Key);
            return new OkObjectResult(result);
        }

        [FunctionName("GetStudentScoreGivenSession")]
        public static async Task<IActionResult> RunningStudentScore(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/StudentScore/{sessionid}")] HttpRequest req,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        string sessionid,
        ILogger log
            )
        {
            var page = await tableClient1.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionid}'").AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page.Values.ToList();
            List<string> result = new();
            Dictionary<string, int> StudentScore = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                foreach (var kvp in dictionary)
                {
                    foreach (var analyserResult in kvp.Value)
                    {
                        if (!StudentScore.ContainsKey(analysisEntity.UserName))
                        {
                            StudentScore[analysisEntity.UserName] = 0;
                        }
                        StudentScore[analysisEntity.UserName] += analyserResult.Verdict;
                    }
                }

            }
            return new OkObjectResult(StudentScore);
        }

        [FunctionName("GetTestScoreGivenSession")]
        public static async Task<IActionResult> RunningTestScore(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = InsightsRoute + "/TestScore/{sessionid}")] HttpRequest req,
        [Table(AnalysisTableName, AnalysisEntity.PartitionKeyName, Connection = ConnectionName)] TableClient tableClient1,
        string sessionid,
        ILogger log
            )
        {
            var page = await tableClient1.QueryAsync<AnalysisEntity>(filter: $"SessionId eq '{sessionid}'").AsPages().FirstAsync();
            List<AnalysisEntity> analysisEntities = page.Values.ToList();
            List<string> result = new();
            Dictionary<string, int> TestScore = new();
            foreach (AnalysisEntity analysisEntity in analysisEntities)
            {
                Dictionary<string, List<AnalyzerResult>> dictionary = InsightsUtility.ConvertAnalysisFileToDictionary(analysisEntity.AnalysisFile);
                foreach (var kvp in dictionary)
                {
                    foreach (var analyserResult in kvp.Value)
                    {
                        if (!TestScore.ContainsKey(analyserResult.AnalyserID))
                        {
                            TestScore[analyserResult.AnalyserID] = 0;
                        }
                        TestScore[analyserResult.AnalyserID] += analyserResult.Verdict;
                    }
                }

            }
            return new OkObjectResult(TestScore);
        }


    }
}
