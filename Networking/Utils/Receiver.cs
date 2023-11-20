using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Networking.Communicator;
using Networking.Events;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;

namespace Networking.Utils
{
    public class Receiver
    {
        //TODO: HANDLE THREAD SLEEP IN RECV LOOP
        private Queue _recvQueue = new();
        private Thread _recvThread;
        private Thread _recvQueueThread;
        private Dictionary<string, NetworkStream> _clientIDToStream;

        private bool _stopThread = false;
        ICommunicator _comm;

        public Receiver(Dictionary<string, NetworkStream> clientIDToStream, ICommunicator comm)
        {
            Console.WriteLine("[Receiver] Init");
            _clientIDToStream = clientIDToStream;
            _recvThread = new Thread(Receive);
            _recvQueueThread = new Thread(RecvLoop);
            _recvThread.Start();
            _recvQueueThread.Start();
            _comm = comm;
        }

        public void Stop()
        {
            Console.WriteLine("[Receiver] Stop");
            _stopThread = true;
            //_recvQueue.Enqueue(new Message(Serializer.Serialize<Data>(
            //    new Data(EventType.StopThread())),, 10 /* TODO */);
            _recvThread.Join();
            _recvQueueThread.Join();
        }

        void Receive()
        {
            Console.WriteLine("[Receiver] Receive starts");
            while (!_stopThread)
            {
                bool ifAval = false;
                foreach (var item in _clientIDToStream)
                {
                    if (item.Value.DataAvailable == true)
                    {
                        ifAval = true;
                        // Read the size of the incoming message
                        byte[] sizeBytes = new byte[sizeof(int)];
                        int sizeBytesRead = item.Value.Read(sizeBytes, 0, sizeof(int));
                        System.Diagnostics.Trace.Assert((sizeBytesRead == sizeof(int)));

                        int messageSize = BitConverter.ToInt32(sizeBytes, 0);

                        // Now read the actual message
                        byte[] receiveData = new byte[messageSize];
                        int totalBytesRead = 0;

                        while (totalBytesRead < messageSize)
                        {
                            sizeBytesRead = item.Value.Read(receiveData, totalBytesRead, messageSize - totalBytesRead);
                            if (sizeBytesRead == 0)
                            { 
                                // Handle the case where the stream is closed or no more data is available
                                break;
                            }
                            totalBytesRead += sizeBytesRead;
                        }

                        System.Diagnostics.Trace.Assert((totalBytesRead == messageSize));

                        string receivedMessage = Encoding.ASCII.GetString(receiveData);
                        Message message = Serializer.Deserialize<Message>(receivedMessage);
                        if (message.ModuleName == ID.GetNetworkingID())
                        {
                            Data data=Serializer.Deserialize<Data>(message.Data);    
                            if (data.EventType == EventType.ClientRegister())
                            {
                                data.Payload = item.Key;
                                message.Data=Serializer.Serialize<Data>(data);
                                //message.Data = item.Key;
                            }
                        }
                        _recvQueue.Enqueue(message, Priority.GetPriority(message.ModuleName) /* fix it */);
                    }
                }
                if (ifAval == false)
                    Thread.Sleep(200);
            }
            Console.WriteLine("[Receiver] Receive stops");
        }

        /*private void handleMessage(Message message)
        {
            foreach (KeyValuePair<string, IEventHandler> pair in _moduleEventMap)
            {
                MethodInfo method = typeof(IEventHandler).GetMethod(message.EventType);
                if (method != null)
                {

                    object[] parameters = new object[] { message };
                    if (message.EventType==EventType.ClientRegister()|| message.EventType == EventType.ClientDeregister())
                    {
                        parameters = new object[] { message ,_clientIDToStream, senderIDToClientID };
                    }
                    try
                    {
                        method.Invoke(pair.Value, parameters);
                    }
                    catch (Exception) { }
                }
                else
                    Console.WriteLine("Method not found");
            }
        }*/

        private void RecvLoop()
        {
            while (!_stopThread)
            {
                if (!_recvQueue.canDequeue())
                {
                    // wait for some time
                    Thread.Sleep(500);
                    continue;
                }

                // Get the next message to send
                Message message = _recvQueue.Dequeue();

                // If the message is a stop message, break out of the loop
                //if (message.StopThread)
                //    break;

                //handleMessage(message);
                if(_comm is Client client)
                {
                    client.HandleMessage(message);
                }
                else if(_comm is Server server)
                {
                    server.HandleMessage(message);
                }
            }
        }
    }
}

