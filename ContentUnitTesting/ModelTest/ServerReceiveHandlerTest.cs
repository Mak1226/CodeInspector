/******************************************************************************
 * Filename    = ServerReceiveHandlerTest.cs
 * 
 * Author      = Sreelakshmi
 *
 * Product     = Analyser
 * 
 * Project     = ContentUnitTesting
 *
 * Description = unit test for ServerReceiveHandler.cs
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
    public class ServerReceiveHandlerTest
    {

        /// <summary>
        /// Test the handling of a valid message by the ServerReceiveHandler.
        /// </summary>
        [TestMethod]
        public void HandleMessageRecv_ValidMessage()

        {

            MockCommunicator mockCommunicator = new();
            MockAnalyzer mockAnalyzer = new();
            ContentServer server = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ServerRecieveHandler handler = new( server );
            Dictionary<string , List<AnalyzerResult>> analyzerResult = new()
             {
                 { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
             };
            AnalyzerResultSerializer serializer = new();

            string encoding = serializer.Serialize( analyzerResult );
            Assert.IsTrue( analyzerResult.ContainsKey( "File1" ) );
            var message = new Message { Data = encoding, SenderID = "senderID" }; // Create a message instance with some data

            // Act
            handler.HandleMessageRecv( message );
            Assert.IsTrue( analyzerResult.ContainsKey( "File1" ) );

            // Assert
            // Assuming analyzerResult is a public property 
            Assert.IsNotNull( server.analyzerResult ); // Assert that analyzerResult is not null

        }
    }
}
