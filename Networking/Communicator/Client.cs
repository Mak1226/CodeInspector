/////
/// Author: 
/////

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;

namespace Networking.Communicator
{
    public class Client : ICommunicator
    {
        private NetworkStream _networkStream;
        private static byte[] ReturnBytes(string serializedObj, string eventType, string destID)
        {
            var data = new Message(serializedObj, eventType, destID);
            string serStr = JsonSerializer.Serialize(data);
            return System.Text.Encoding.ASCII.GetBytes(serStr);
        }

        void ICommunicator.Send(string serializedObj, string eventType, string destID)
        {
            Trace.WriteLine("[Client] Send" + serializedObj + " " + eventType + " " + destID);
            _networkStream.Write(ReturnBytes(serializedObj, eventType, destID));
            //throw new NotImplementedException();
        }

        string ICommunicator.Start(string? destIP, int? destPort)
        {
            Trace.WriteLine("[Client] Start" + destIP + " " + destPort);
            TcpClient tcpClient = new();
            if(destIP!=null && destPort!=null)
            tcpClient.Connect(destIP, destPort.Value);
            _networkStream = tcpClient.GetStream();
            return "";
            //throw new NotImplementedException();
        }

        void ICommunicator.Stop()
        {
            Trace.WriteLine("[Client] Stop");
            //throw new NotImplementedException();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Client] Subscribe");
            //throw new NotImplementedException();
        }
    }
}

