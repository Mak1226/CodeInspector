/////
/// Author: 
/////

using System;
namespace Networking.Communicator
{
	public class Message
    {
        public string SerializedObj { get; }
        public string EventType { get; }
        public string? DestID { get; }

        public Message(string serializedObj, string eventType)
        {
            SerializedObj = serializedObj;
            EventType = eventType;
            DestID = null;
        }

        public Message(string serializedObj, string eventType, string destID)
        {
            SerializedObj = serializedObj;
            EventType = eventType;
            DestID = destID;
        }
    }

}

