/******************************************************************************
* Filename    = UploadApiTests.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description = Testing the exceptions in uploadApi
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerlessFunc;

namespace CloudUnitTests
{
    [TestClass]
    public class UploadApiTests
    {
        private UploadApi _uploadApi;

        [TestInitialize]
        public void Setup()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new HttpResponseMessage( HttpStatusCode.OK ) ) );

            string sessionUrl = "http://fake-session-url";
            string submissionUrl = "http://fake-submission-url";
            string analysisUrl = "http://fake-analysis-url";

            _uploadApi = new UploadApi( fakeHttpClient , sessionUrl , submissionUrl , analysisUrl );
        }

        [TestMethod]
        public async Task PostSessionAsync_NetworkError_ReturnsDefault()
        {

            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var uploadApi = new UploadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            SessionEntity result = await uploadApi.PostSessionAsync( new SessionData() );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task PostSubmissionAsync_NetworkError_ReturnsDefault()
        {
            
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var uploadApi = new UploadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            SubmissionEntity result = await uploadApi.PostSubmissionAsync( new SubmissionData() );

            Assert.IsNull( result );
        }

        [TestMethod]
        public async Task PostAnalysisAsync_NetworkError_ReturnsDefault()
        {
           
            var fakeHttpClient = new HttpClient( new FakeHttpMessageHandler( new Exception( "Simulated network error" ) ) );
            var uploadApi = new UploadApi( fakeHttpClient , "http://fake-session-url" , "http://fake-submission-url" , "http://fake-analysis-url" );

            AnalysisEntity result = await uploadApi.PostAnalysisAsync( new AnalysisData() );

            Assert.IsNull( result );
        }
    }

}
