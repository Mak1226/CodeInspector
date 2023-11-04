﻿/////
/// Author:
/////

using System;
using System.Diagnostics;
using System.Text.Json;
using System.Net.Sockets;
using System.Collections.Generic;
using Networking.Queues;
using System.Reflection;
using System.Net;

namespace Networking.Communicator
{
    public class Server : ICommunicator
    {
        private Queue _recvQueue = new();
        private Thread _recvThread;
        private Thread _recvQueueThread;
        private Thread _listenThread;
        Dictionary<string, NetworkStream> _clientIDToStream = new();
        private Thread _sendThread;
        private Queue _sendQueue = new();
        private Dictionary<string, IEventHandler> _moduleEventMap = new();

        private static byte[] ReturnBytes(string serializedObj, string eventType)
        {
            var data = new Message(serializedObj, eventType);
            string serStr = JsonSerializer.Serialize(data);
            return System.Text.Encoding.ASCII.GetBytes(serStr);
        }

        public void Send(string serializedObj, string eventType, string destID)
        {
            Trace.WriteLine("[Server] Send" + serializedObj + " " + eventType + " " + destID);
            _sendQueue.Enqueue(JsonSerializer.Serialize(new Message(serializedObj, eventType, destID)), 1 /* fix it */);
        }

         public string Start(string? destIP, int? destPort)
        {



            _listenThread = new Thread(AcceptConnection);
            // Start the send thread
            _sendThread = new Thread(SendLoop);
            _recvThread = new Thread(Receive);
            _recvQueueThread = new Thread(RecvLoop);
            _listenThread.Start();
            _sendThread.Start();
            _recvThread.Start();
            _recvQueueThread.Start();
            Console.Write("exiting");
            // TODO: Add accept connections by client, create nwstream, and add to dict
            return "";
            //throw new NotImplementedException();
        }

        public void Stop()
        {
            Trace.WriteLine("[Server] Stop");

            // Signal the send thread to stop
            _sendQueue.Enqueue("StopIt", 10 /* fix it */);

            // Wait for the send thread to stop
            _sendThread.Join();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Server] Subscribe");
            _moduleEventMap.Add(moduleName, eventHandler);
            throw new NotImplementedException();
        }
        void Receive()
        {
            while (true)
            {
                bool ifAval = false;
                foreach (var item in _clientIDToStream)
                {
                    if (item.Value.DataAvailable == true)
                    {
                        ifAval = true;
                        byte[] receiveData = new byte[1024];
                        int bytesRead = item.Value.Read(receiveData, 0, receiveData.Length);
                        string receivedMessage = System.Text.Encoding.ASCII.GetString(receiveData, 0, bytesRead);
                        _recvQueue.Enqueue(receivedMessage, 1 /* fix it */);
                    }
                }
                if (ifAval == false)
                    Thread.Sleep(200);
            }
        }
        void AcceptConnection()
        {

            TcpListener serverListener = new TcpListener(IPAddress.Any, 12345);
            serverListener.Start();
            IPEndPoint localEndPoint = (IPEndPoint)serverListener.LocalEndpoint;
            Trace.WriteLine("[Server] Start");
            Console.WriteLine("Server is listening on:");
            Console.WriteLine("IP Address: " + localEndPoint.Address);
            Console.WriteLine("Port: " + localEndPoint.Port);
            string clientID = "A";

            while (true)
            {
                Console.WriteLine("waiting for connection");
                TcpClient client = serverListener.AcceptTcpClient();
                
                // Create a NetworkStream for the client
                NetworkStream stream = client.GetStream();
                _clientIDToStream.Add(clientID, stream);
                clientID += 'A';
                Trace.WriteLine("client connected");
                // Handle communication with the client using 'stream'
                // (e.g., read and process incoming data, send responses)
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