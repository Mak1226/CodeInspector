using System;
using Networking.Communicator;
using Networking.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentUnitTesting.ContentTest
{
    public class MockCommunicator : ICommunicator
    {
        private int _sentMessageCounter;
        public string serializedObj { get; private set; }
        public MockCommunicator()
        {
            _sentMessageCounter = 0;
        }
        public void Send(string serializedObj, string eventType, string? destID = null)
        {
            _sentMessageCounter++;
            this.serializedObj = serializedObj;
        }
        public int CheckMessageCount()
        {
            return _sentMessageCounter;
        }
        public string Start(string? destIP, int? destPort, string senderID)
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
