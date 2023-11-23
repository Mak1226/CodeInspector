/******************************************************************************
 * Filename     = MockCommunicator.cs
 * 
 * Author       = Lekshmi
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = MockCommunicator for Unit Testing
*****************************************************************************/
using Networking.Communicator;
using Networking.Events;

namespace ContentUnitTesting.ContentClientServerTest
{
    /// <summary>
    /// A mock implementation of the ICommunicator interface for unit testing purposes.
    /// </summary>
    internal class MockCommunicator : ICommunicator
    {
        private string? _encoding;
        /// <summary>
        /// Sends serialized data to a specified module and destination ID.
        /// </summary>
        /// <param name="serializedData">The serialized data to be sent.</param>
        /// <param name="moduleName">The name of the destination module.</param>
        /// <param name="destId">The destination ID.</param>
        public void Send(string serializedData, string moduleName, string destId)
        {
            _encoding = serializedData;
        }

        /// <summary>
        /// Retrieves the last encoding value set by the Send method.
        /// </summary>
        /// <returns>The last encoding value.</returns>
        public string GetEncoding()
        {
            return _encoding;
        }

        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public void Send(string serializedData, string destId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public string Start(string? destIP, int? destPort, string senderId, string moduleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            return;
        }
    }
}
