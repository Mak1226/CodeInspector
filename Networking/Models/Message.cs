/////
/// Author: 
/////

using System;

namespace Networking.Models
{
    public class Message
    {
        public string Data { get; set; }
        public string EventType { get; set; }
        public bool StopThread { get; set; }
        public string? DestID { get; set; }
        public string? SenderID { get; set; }
        public Message()
        {
            Data = "";
            EventType = "";
            DestID = null;
            StopThread = false;
        }

        //public Message(string serializedObj, string eventType)
        //{
        //    Data = serializedObj;
        //    EventType = eventType;
        //    DestID = null;
        //}

        public Message(string serializedObj, string eventType, string destID, string? senderID)
        {
            Data = serializedObj;
            EventType = eventType;
            DestID = destID;
            StopThread = false;
            SenderID = senderID;
        }

        public Message(bool stop)
        {
            Data = "";
            EventType = "";
            DestID = null;
            StopThread = stop;
        }
    }

}

