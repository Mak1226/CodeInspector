/////
/// Author: 
/////

using System;
namespace Networking.Communicator
{
	public class Message
    {
        public string SerializedObj { get; set; }
        public string EventType { get; set; }
        public string? DestID { get; set; }
        public Message() {
            this.SerializedObj = null;
            this.EventType = null;
            this.DestID = null; 
        }

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

