using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Utils
{
    public static class EventType
    {
        /*    public string HandleFile(string data);
            public string HandleChatMessage(string data);
            public string HandleAnalyserResult(string data);
            public string HandleConnectionRequest(string data);
            public string HandleClientJoined(string data);
            public string HandleClientLeft(string data);*/
        public static string AnalyseFile()
        {
            return "HandleFile";
        }
        public static string ChatMessage()
        {
            return "HandleChatMessage";
        }
        public static string AnalyserResult()
        {
            return "HandleAnalyserResult";
        }
        public static string NewClientJoined()
        {
            return "HandleClientJoined";
        }
        public static string ClientLeft()
        {
            return "HandleClientLeft";
        }
        public static string ClientRegister()
        {
            return "HandleClientRegister";
        }
        public static string ClientDeregister()
        {
            return "HandleClientDeregister";
        }
    }
}
