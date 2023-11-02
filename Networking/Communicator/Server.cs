/////
/// Author:
/////

using System;
using System.Diagnostics;
using System.Text.Json; 
using System.Net.Sockets;
using System.Collections.Generic;
using Networking.Queues;

namespace Networking.Communicator
{
    public class Server : ICommunicator
    {
        Dictionary<string, NetworkStream> _clientIDToStream = new();
        private Thread _sendThread;
        private Queue _sendQueue = new();
        private static byte[] ReturnBytes(string serializedObj, string eventType)
        {
            var data = new Message(serializedObj, eventType);
            string serStr = JsonSerializer.Serialize(data);
            return System.Text.Encoding.ASCII.GetBytes(serStr);
        }

        void ICommunicator.Send(string serializedObj, string eventType, string destID)
        {
            Trace.WriteLine("[Server] Send" + serializedObj + " " + eventType + " " + destID);
            _sendQueue.Enqueue(JsonSerializer.Serialize(new Message(serializedObj, eventType, destID)), 1 /* fix it */);
        }

        string ICommunicator.Start(string? destIP, int? destPort)
        {
            Trace.WriteLine("[Server] Start");
            // Start the send thread
            _sendThread = new Thread(SendLoop);
            _sendThread.Start();
            // TODO: Add accept connections by client, create nwstream, and add to dict
            return "";
            //throw new NotImplementedException();
        }

        void ICommunicator.Stop()
        {
            Trace.WriteLine("[Server] Stop");

            // Signal the send thread to stop
            _sendQueue.Enqueue(JsonSerializer.Serialize("StopIt"), 10 /* fix it */);

            // Wait for the send thread to stop
            _sendThread.Join();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Server] Subscribe");
            throw new NotImplementedException();
        }

        private void SendLoop()
        {
            while (true)
            {
                if (!_sendQueue.canDequeue())
                {
                    // wait for some time
                    Thread.Sleep(500);
                    continue;
                }

                // Get the next message to send
                string message = _sendQueue.Dequeue();


                // If the message is a stop message, break out of the loop
                try
                {
                    if (JsonSerializer.Deserialize<string>(message) == "StopIt")
                        break;
                }
                catch
                {
                    Message msg = JsonSerializer.Deserialize<Message>(message);

                    if (msg.DestID != "brodcast")
                    {
                        byte[] messagetxt = ReturnBytes(msg.SerializedObj, msg.EventType);
                        _clientIDToStream[msg.DestID].Write(messagetxt);
                    }
                    else
                    {   // send to all clients
                        foreach (KeyValuePair<string, NetworkStream> pair in _clientIDToStream)
                        {
                            byte[] messagetxt = ReturnBytes(msg.SerializedObj, msg.EventType);
                            pair.Value.Write(messagetxt);
                        }
                    }
                }
            }
        }
    }
}
