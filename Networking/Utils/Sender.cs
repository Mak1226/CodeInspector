using System.Net.Sockets;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;

namespace Networking.Utils
{
    public class Sender
    {
        private Queue _sendQueue = new();
        private Thread _sendThread;
        private bool _isClient;
        private Dictionary<string, NetworkStream> clientIDToStream;
        Dictionary<string, string> senderIDToClientID;


        public Sender(Dictionary<string, NetworkStream> clientIDToStream, Dictionary<string, string> senderIDToClientID, bool isClient)
        {
            this.senderIDToClientID=senderIDToClientID;
            _isClient = isClient;
            Console.WriteLine("[Sender] Init");
            this.clientIDToStream = clientIDToStream;
            _sendThread = new Thread(SendLoop);
            _sendThread.Start();
        }

        public void Stop()
        {
            Console.WriteLine("[Sender] Stop");
            _sendQueue.Enqueue(new Message(stop: true), 10 /* TODO */);
            _sendThread.Join();
        }

        public void Send(Message message)
        {
            // NOTE: destID should be in line with the dict passed 
            _sendQueue.Enqueue(message, Priority.GetPriority(message.EventType)/* TODO */);
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

                string serStr = Serializer.Serialize(message);
                byte[] messagebytes = System.Text.Encoding.ASCII.GetBytes(serStr);
                if (_isClient == true)
                {
                    clientIDToStream[ID.GetServerID()].Write(messagebytes);
                }
                else
                {
                    if (message.DestID == ID.GetBroadcastID())
                    {
                        foreach (KeyValuePair<string, NetworkStream> pair in clientIDToStream)
                        {
                            pair.Value.Write(messagebytes);
                        }
                    }
                    else
                    {
                        clientIDToStream[senderIDToClientID[message.DestID]].Write(messagebytes);
                    }
                }

            }
        }
    }
}

