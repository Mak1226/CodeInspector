using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using Networking.Models;
using Networking.Queues;

namespace Networking.Utils
{
    public class Priority
    {
        public static int GetPriority(string moduleName)
        {
            int priority =10;
            if (moduleName == ID.GetNetworkingBroadcastID())
            {
                priority = 0;
            }
            else if (moduleName == ID.GetNetworkingID())
                {
                    priority = 1;
                }
            return priority;
        }
    }
}

