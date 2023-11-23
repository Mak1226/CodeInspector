/******************************************************************************
* Filename    = InsightsApiTests.cs
*
* Author      = Sahil
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description =  Testing the exceptions in insightsApi
*****************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ServerlessFunc;

namespace CloudUnitTests
{
    [TestClass]
    public class InsightsApiTests
    {
        private InsightsApi _insightsApi;

        [TestInitialize]
        public void Setup()
        {
            
            var fakeHttpClient = new Mock<HttpClient>( new FakeHttpMessageHandler( new HttpResponseMessage( HttpStatusCode.OK ) ) );

            string insightsUrl = "http://fake-insights-url";

            _insightsApi = new InsightsApi( fakeHttpClient.Object , insightsUrl );
        }

        [TestMethod]
        public async Task CompareTwoSessions_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            List<Dictionary<string , int>> result = await insightsApi.CompareTwoSessions( "fake-session-id-1" , "fake-session-id-2" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetFailedStudentsGivenTest_NetworkError_ReturnsDefault()
        {
          
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            List<string> result = await insightsApi.GetFailedStudentsGivenTest( "fake-hostname" , "fake-test-name" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task RunningAverageOnGivenTest_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            List<double> result = await insightsApi.RunningAverageOnGivenTest( "fake-hostname" , "fake-test-name" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task RunningAverageOnGivenStudent_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            List<double> result = await insightsApi.RunningAverageOnGivenStudent( "fake-hostname" , "fake-student-name" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task RunningAverageAcrossSessoins_NetworkError_ReturnsDefault()
        {

            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            List<double> result = await insightsApi.RunningAverageAcrossSessoins( "fake-hostname" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task UsersWithoutAnalysisGivenSession_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            List<string> result = await insightsApi.UsersWithoutAnalysisGivenSession( "fake-session-id" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetStudentScoreGivenSession_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );
            Dictionary<string , int> result = await insightsApi.GetStudentScoreGivenSession( "fake-session-id" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetTestScoreGivenSession_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );

            Dictionary<string , int> result = await insightsApi.GetTestScoreGivenSession( "fake-session-id" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task GetBestWorstGivenSession_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var insightsApi = new InsightsApi( fakeHttpClient , "http://fake-insights-url" );
 
            List<string> result = await insightsApi.GetBestWorstGivenSession( "fake-session-id" );

            Assert.IsNull( result );
        }
    }
}
