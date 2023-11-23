using Microsoft.VisualStudio.TestTools.UnitTesting;
using Content.Model;
using Networking.Models;

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
        public void HandleMessageRecv_ValidMessage_ReturnsEmptyString()
        {
            // Arrange: Create a test message
            Message message = new( "Test data" );

            // Act: Call the HandleMessageRecv method and capture the return value
            string result = _handler.HandleMessageRecv( message );

            // Assert: Verify that the method returns an empty string
            Assert.IsTrue( string.IsNullOrEmpty( result ) );
        }

        // Add more test cases to cover different scenarios and edge cases
    }
}
