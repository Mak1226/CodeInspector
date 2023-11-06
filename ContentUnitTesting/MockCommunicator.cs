using System;
using Networking.Communicator;
using Networking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentUnitTesting
{
    public class MockCommunicator : ICommunicator
    {
        private int _sentMessageCounter;
        public MockCommunicator()
        {
            _sentMessageCounter = 0;
        }
        public void Send(string serializedObj, string eventType, string? destID = null)
        {
            _sentMessageCounter++;
        }
        public int CheckMessageCount()
        {
            return _sentMessageCounter;
        }
        public string Start(string? destIP = null, int? destPort = null)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            throw new NotImplementedException();
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
