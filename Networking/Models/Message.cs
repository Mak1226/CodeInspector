/////
/// Author: 
/////

using System;

namespace Networking.Models
{
    public class Message
    {
        private string _v;

        public string Data { get; set; }
        public string ModuleName { get; set; }
        public string DestID { get; set; }
        public string SenderID { get; set; }
        public Message()
        {
            Data = "";
            DestID = "";
            SenderID = "";
            ModuleName = "";
        }

        public Message(string Data, string ModuleName, string destID, string senderID)
        {
            this.ModuleName = ModuleName;
            this.Data = Data;
            DestID = destID;
            SenderID = senderID;
        }

        public Message( string v )
        {
            _v = v;
        }
    }

}

