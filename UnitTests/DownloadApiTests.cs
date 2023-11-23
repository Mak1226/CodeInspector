/******************************************************************************
* Filename    = DownloadApiTests.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description = Testing the exceptions in downloadApi
*****************************************************************************/

using CloudUnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerlessFunc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudUnitTests
{
    [TestClass]
    public class DownloadApiTests
    {
        private DownloadApi ?_downloadApi;

        [TestInitialize]
        public void Setup()
        {
           
            var mockHttpClient = new Mock<HttpClient>( new FakeHttpMessageHandler( new HttpResponseMessage( HttpStatusCode.OK ) ) );

            string sessionUrl = "http://fake-session-url";
            string submissionUrl = "http://fake-submission-url";
            string analysisUrl = "http://fake-analysis-url";

            _downloadApi = new( mockHttpClient.Object , sessionUrl , submissionUrl , analysisUrl );
        }

        [TestMethod]
        public async Task GetSessionsByHostNameAsync_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            IReadOnlyList<SessionEntity> result = await downloadApi.GetSessionsByHostNameAsync( "fake-host" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetSubmissionByUserNameAndSessionIdAsync_NetworkError_ReturnsDefault()
        {
           
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            byte[] result = await downloadApi.GetSubmissionByUserNameAndSessionIdAsync( "fake-username" , "fake-session-id" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetAnalysisByUserNameAndSessionIdAsync_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            IReadOnlyList<AnalysisEntity> result = await downloadApi.GetAnalysisByUserNameAndSessionIdAsync( "fake-username" , "fake-session-id" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetAnalysisBySessionIdAsync_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            IReadOnlyList<AnalysisEntity> result = await downloadApi.GetAnalysisBySessionIdAsync( "fake-session-id" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task DeleteAllSessionsAsync_NetworkError_NoExceptionThrown()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            await downloadApi.DeleteAllSessionsAsync();
        }

        [TestMethod]
        public async Task DeleteAllSubmissionsAsync_NetworkError_NoExceptionThrown()
        {
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            await downloadApi.DeleteAllSubmissionsAsync();
        }

        [TestMethod]
        public async Task DeleteAllAnalysisAsync_NetworkError_NoExceptionThrown()
        {

            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var downloadApi = new DownloadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            await downloadApi.DeleteAllAnalysisAsync();
        }
    }
}
