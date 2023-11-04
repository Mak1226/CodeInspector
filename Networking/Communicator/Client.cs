/////
/// Author: 
/////

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using Networking.Queues;

namespace Networking.Communicator
{
    public class Client : ICommunicator
    {
        private NetworkStream _networkStream;
        private Thread _sendThread;
        private Thread _recvThread;
        private Thread _recvQueueThread;
        private Queue _sendQueue = new();

        private Queue _recvQueue = new();

        private Dictionary<string, IEventHandler> _moduleEventMap = new();
        public void Send(string serializedObj, string eventType, string destID)
        {
            Trace.WriteLine("[Client] Send" + serializedObj + " " + eventType + " " + destID);
            _sendQueue.Enqueue(JsonSerializer.Serialize(new Message(serializedObj, eventType, destID)), 1 /* fix it */);
        }


        public string Start(string? destIP, int? destPort)
        {
            Console.WriteLine("[Client] Start" + destIP + " " + destPort);
            TcpClient tcpClient = new();
            if (destIP != null && destPort != null)
                tcpClient.Connect(destIP, destPort.Value);
            _networkStream = tcpClient.GetStream();

            Console.WriteLine("[Client] Starting threads");
            // Start the send thread
            _sendThread = new Thread(SendLoop);
            _sendThread.Start();
            _recvThread = new Thread(Receive);
            _recvQueueThread = new Thread(RecvLoop);
            _recvThread.Start();
            _recvQueueThread.Start();
            Console.WriteLine("[Client] Started");
            return "";
        }

        public void Stop()
        {
            Trace.WriteLine("[Client] Stop");

            // Signal the send thread to stop
            _sendQueue.Enqueue("StopIt", 10 /* fix it */);
            _recvQueue.Enqueue("StopIt", 10);

            // Wait for the send thread to stop
            _sendThread.Join();
            _networkStream.Close();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Client] Subscribe");
            _moduleEventMap.Add(moduleName, eventHandler);
            throw new NotImplementedException();
        }
        void Receive()
        {
            while (true)
            {
                if (_networkStream.DataAvailable == true)
                {
                    byte[] receiveData = new byte[1024];
                    int bytesRead = _networkStream.Read(receiveData, 0, receiveData.Length);
                    string receivedMessage = System.Text.Encoding.ASCII.GetString(receiveData, 0, bytesRead);
                    _recvQueue.Enqueue(receivedMessage, 1 /* fix it */);
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
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
                if (message == "StopIt")
                    break;
                _networkStream.Write(System.Text.Encoding.ASCII.GetBytes(message));


            }
        }
        private void handleMessage(Message message)
        {
            foreach (KeyValuePair<string, IEventHandler> pair in _moduleEventMap)
            {
                MethodInfo method = typeof(IEventHandler).GetMethod(message.EventType);
                if (method != null)
                {
                    object[] parameters = new object[] { message.SerializedObj };
                    method.Invoke(pair.Value, parameters);
                }
                else
                    Console.WriteLine("Method not found");
            }
        }
        private void RecvLoop()
        {
            while (true)
            {
                if (!_recvQueue.canDequeue())
                {
                    // wait for some time
                    Thread.Sleep(500);
                    continue;
                }

                // Get the next message to send
                string message = _recvQueue.Dequeue();

                // If the message is a stop message, break out of the loop
                try
                {
                    if (message == "StopIt")
                        break;
                    Message message1 = JsonSerializer.Deserialize<Message>(message);
                    handleMessage(message1);
                }
                catch
                {
                    // Send the serialized message
                    throw new Exception();
                }
            }
        }

    }
}

