using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using Networking.Queues;

namespace Networking.Utils
{
	public class Sender
	{
        private Queue _sendQueue = new();
        private string BROADCAST = "broadcast";
        private Thread _sendThread;
        private bool _isClient;
        private Dictionary<string, NetworkStream> clientIDToStream;

        public Sender(Dictionary<string, NetworkStream> clientIDToStream,bool isClient)
		{
            _isClient = isClient;
            Console.WriteLine("[Sender] Init");
            this.clientIDToStream = clientIDToStream;
            _sendThread = new Thread(SendLoop);
            _sendThread.Start();
        }

        public void Stop()
        {
            Console.WriteLine("[Sender] Stop");
            _sendQueue.Enqueue(new Message(stop:true), 10 /* TODO */);
            _sendThread.Join();
        }

        public void Send(string serializedObj, string eventType, string destID,string senderID)
        {
            // NOTE: destID should be in line with the dict passed 

            Console.WriteLine("[Sender] Send" + serializedObj + " " + eventType + " " + destID);
            _sendQueue.Enqueue(new Message(serializedObj, eventType, destID,senderID), 1 /* TODO */);
        }

        public void SendLoop()
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
                Message message = _sendQueue.Dequeue();

                // If the message is a stop message, break out of the loop
                if (message.StopThread)
                    break;

                string serStr = JsonSerializer.Serialize(message);
                byte[] messagebytes = System.Text.Encoding.ASCII.GetBytes(serStr);
                if (_isClient == true)
                {
                    clientIDToStream["server"].Write(messagebytes);
                }
                else
                {
                    if (message.DestID == "BROADCAST")
                    {
                        foreach (KeyValuePair<string, NetworkStream> pair in clientIDToStream)
                        {
                            pair.Value.Write(messagebytes);
                        }
                    }
                    else
                    {
                        clientIDToStream[message.DestID].Write(messagebytes);
                    }
                }
                
            }
        }
    }
}

