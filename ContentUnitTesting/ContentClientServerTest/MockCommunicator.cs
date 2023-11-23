using Networking.Communicator;
using Networking.Events;

namespace ContentUnitTesting.ContentClientServerTest
{
    internal class MockCommunicator : ICommunicator
    {
        private string? _encoding;

        public void Send(string serializedData, string moduleName, string destId)
        {
            _encoding = serializedData;
        }

        public string GetEncoding()
        {
            return _encoding;
        }
        public void Send(string serializedData, string destId)
        {
            throw new NotImplementedException();
        }

        public string Start(string? destIP, int? destPort, string senderId, string moduleName)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            return;
        }
    }
}
