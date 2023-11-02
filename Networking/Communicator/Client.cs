/////
/// Author: 
/////

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using Networking.Queues;

namespace Networking.Communicator
{
    public class Client : ICommunicator
    {
        private NetworkStream _networkStream;
        private Thread _sendThread;
        private Queue _sendQueue = new ();

        void ICommunicator.Send(string serializedObj, string eventType, string destID)
        {
            Trace.WriteLine("[Client] Send" + serializedObj + " " + eventType + " " + destID);
            _sendQueue.Enqueue(JsonSerializer.Serialize(new Message(serializedObj, eventType, destID)), 1 /* fix it */);
        }

        string ICommunicator.Start(string? destIP, int? destPort)
        {
            Trace.WriteLine("[Client] Start" + destIP + " " + destPort);
            TcpClient tcpClient = new();
            if(destIP!=null && destPort!=null)
            tcpClient.Connect(destIP, destPort.Value);
            _networkStream = tcpClient.GetStream();

            // Start the send thread
            _sendThread = new Thread(SendLoop);
            _sendThread.Start();

            return "";
        }

        void ICommunicator.Stop()
        {
            Trace.WriteLine("[Client] Stop");

            // Signal the send thread to stop
            _sendQueue.Enqueue(JsonSerializer.Serialize("StopIt"), 10 /* fix it */);

            // Wait for the send thread to stop
            _sendThread.Join();

            _networkStream.Close();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Client] Subscribe");
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
                    if (JsonSerializer.Deserialize<string> (message) == "StopIt")
                    break;
                }
                catch
                {
                    // Send the serialized message
                    _networkStream.Write(System.Text.Encoding.ASCII.GetBytes(message));
                }
            }
        }

    }
}

