/******************************************************************************
 * Filename    = ClientReceiveHandlerTest.cs
 * 
 * Author      = Sreelakshmi
 *
 * Product     = Analyser
 * 
 * Project     = ContentUnitTesting
 *
 * Description = unit test for ClientReceiveHandler.cs
 *              
 *****************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Content.Model;
using Networking.Models;
using Content.Encoder;
using Analyzer;
using Newtonsoft.Json.Linq;
using Networking.Communicator;
using ContentUnitTesting.ContentClientServerTest;

namespace ContentUnitTesting.ModelTest
{
    [TestClass]
    public class ClientReceiveHandlerTest
    {
        /// <summary>
        /// Test the handling of a valid message by the ClientReceiveHandler.
        /// </summary>

        [TestMethod]
        public void HandleMessageRecv_ValidMessage()

        {
            MockCommunicator mockCommunicator = new();
            ContentClient contentClient = new( mockCommunicator , "sessionId" );
            ClientRecieveHandler handler = new( contentClient );
            
            Dictionary<string , List<AnalyzerResult>> analyzerResult = new()
             {
                 { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
             };
            AnalyzerResultSerializer serializer = new();

            string encoding = serializer.Serialize( analyzerResult );
           Assert.IsTrue( analyzerResult.ContainsKey( "File1" ) );
            var message = new Message { Data = encoding }; // Create a message instance with some data

            // Act
            handler.HandleMessageRecv( message );
            Assert.IsTrue( analyzerResult.ContainsKey( "File1" ) );

            // Assert
            // Assuming analyzerResult is a public property in ClientReceiveHandler
            Assert.IsNotNull( contentClient.analyzerResult ); // Assert that analyzerResult is not null
            
        }

        // Add more test cases to cover different scenarios and edge cases
    }
}
