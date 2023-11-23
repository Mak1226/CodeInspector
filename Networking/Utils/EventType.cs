using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Utils
{
    public static class EventType
    {
        public static string ChatMessage()
        {
            return "HandleChatMessage";
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
        public static string ServerLeft()
        {
            return "HandleServerLeft";
        }   
    }
}
