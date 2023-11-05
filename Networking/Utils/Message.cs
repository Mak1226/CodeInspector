/////
/// Author: 
/////

using System;
namespace Networking.Utils
{
	public class Message
    {
        public string SerializedObj { get; set; }
        public string EventType { get; set; }
        public bool StopThread { get; set; }
        public string? DestID { get; set; }
        public string? SenderID { get;set; }
        public Message() {
            this.SerializedObj = "";
            this.EventType = "";
            this.DestID = null;
            StopThread = false;
        }

        //public Message(string serializedObj, string eventType)
        //{
        //    SerializedObj = serializedObj;
        //    EventType = eventType;
        //    DestID = null;
        //}

        public Message(string serializedObj, string eventType, string destID, string? senderID)
        {
            SerializedObj = serializedObj;
            EventType = eventType;
            DestID = destID;
            StopThread = false;
            SenderID = senderID;
        }

        public Message(bool stop)
        {
            SerializedObj = "";
            EventType = "";
            DestID = null;
            StopThread = stop;
        }
    }

}

