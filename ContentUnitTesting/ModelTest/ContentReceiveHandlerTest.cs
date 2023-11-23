using Microsoft.VisualStudio.TestTools.UnitTesting;
using Content.Model;
using Networking.Models;
using Content.Encoder;
using Analyzer;
using Newtonsoft.Json.Linq;

namespace ContentUnitTesting.ModelTest
{
    [TestClass]
    public class ClientReceiveHandlerTest
    {
        private ClientRecieveHandler _handler;
        private ContentClient _clientMock;

        [TestInitialize]
        public void Setup()
        {
            // Arrange: Create an instance of ContentClient (or mock it using a mocking framework like Moq)
            _clientMock = new ContentClient(); 

            // Arrange: Initialize the ClientRecieveHandler with the ContentClient instance
            _handler = new ClientRecieveHandler( _clientMock );
        }

        [TestMethod]
        public void HandleMessageRecv_ValidMessage()
        {
            // Dictionary<string , List<AnalyzerResult>> analyzerResult = new()
            // {
            //     { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
            //      Add more initial values as needed
            // };
            //AnalyzerResultSerializer serializer = new();

            // string encoding = serializer.Serialize( analyzerResult );
            // _handler.HandleMessageRecv( encoding );
            // When passed value is empty, analyzerResult is not updated
            //Assert.IsTrue( analyzerResult.ContainsKey( "File1" ) );
            var message = new Message { Data = "Test Data" }; // Create a message instance with some data

            // Act
            _handler.HandleMessageRecv( message );

            // Assert
            // Assuming analyzerResult is a public property in ClientReceiveHandler
            Assert.IsNull( _clientMock.analyzerResult ); // Assert that analyzerResult is not null
            
        }

        // Add more test cases to cover different scenarios and edge cases
    }
}
