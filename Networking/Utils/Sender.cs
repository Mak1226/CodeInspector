using System.Net.Sockets;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;

namespace Networking.Utils
{
    public class Sender
    {
        //TODO: HANDLE THREAD SLEEP
        private Queue _sendQueue = new();
        private Thread _sendThread;
        private bool _isClient;
        private Dictionary<string, NetworkStream> clientIDToStream;
        Dictionary<string, string> senderIDToClientID;
        private bool _stopThread;


        public Sender(Dictionary<string, NetworkStream> clientIDToStream, Dictionary<string, string> senderIDToClientID, bool isClient)
        {
            _stopThread = false;
            this.senderIDToClientID=senderIDToClientID;
            _isClient = isClient;
            Console.WriteLine("[Sender] Init");
            this.clientIDToStream = clientIDToStream;
            _sendThread = new Thread(SendLoop)
            {
                IsBackground = true
            };
            _sendThread.Start();
        }

        public void Stop()
        {

            Console.WriteLine("[Sender] Stop");
            _stopThread = true;
            //_sendQueue.Enqueue(new Message(stop: true), 10 /* TODO */);
            _sendThread.Join();
        }

        public void Send(Message message)
        {
            // NOTE: destID should be in line with the dict passed 
            _sendQueue.Enqueue(message, Priority.GetPriority(message.ModuleName)/* TODO */);
        }

        public void SendLoop()
        {
            while (!_stopThread)
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
                //if (message.StopThread)
                //    break;

                string serStr = Serializer.Serialize(message);
                byte[] messagebytes = System.Text.Encoding.ASCII.GetBytes(serStr);
                int messageSize = messagebytes.Length;
                if (_isClient == true)
                {
                    clientIDToStream[ID.GetServerID()].Write(BitConverter.GetBytes(messageSize), 0, sizeof(int));
                    clientIDToStream[ID.GetServerID()].Write(messagebytes);
                    clientIDToStream[ID.GetServerID()].Flush();
                }
                else
                {
                    if (message.DestID == ID.GetBroadcastID())
                    {
                        foreach (KeyValuePair<string, NetworkStream> pair in clientIDToStream)
                        {
                            pair.Value.Write(BitConverter.GetBytes(messageSize), 0, sizeof(int));
                            pair.Value.Write(messagebytes);
                            pair.Value.Flush();
                        }
                    }
                    else
                    {
                        clientIDToStream[senderIDToClientID[message.DestID]].Write(BitConverter.GetBytes(messageSize), 0, sizeof(int));
                        clientIDToStream[senderIDToClientID[message.DestID]].Write(messagebytes);
                        clientIDToStream[senderIDToClientID[message.DestID]].Flush();
                    }
                }

            }
        }
    }
}

