using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using Networking.Queues;

namespace Networking.Utils
{
	public class Receiver
	{
		private Queue _recvQueue = new();
        private Thread _recvThread;
        private Thread _recvQueueThread;
        private Dictionary<string, NetworkStream> _clientIDToStream;
		private Dictionary<string, IEventHandler> _moduleEventMap;
        private bool _stopThread = false;

        public Receiver(Dictionary<string, NetworkStream> clientIDToStream, Dictionary<string, IEventHandler> moduleEventMap)
		{
            Console.WriteLine("[Receiver] Init");
            _clientIDToStream = clientIDToStream;
			_moduleEventMap = moduleEventMap;
            _recvThread = new Thread(Receive);
            _recvQueueThread = new Thread(RecvLoop);
            _recvThread.Start();
            _recvQueueThread.Start();
        }

        public void Stop()
        {
            Console.WriteLine("[Receiver] Stop");
            _stopThread = true;
            _recvQueue.Enqueue(new Message(stop: true), 10 /* TODO */);
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
                        byte[] receiveData = new byte[1024];
                        int bytesRead = item.Value.Read(receiveData, 0, receiveData.Length);

                        string receivedMessage = System.Text.Encoding.ASCII.GetString(receiveData, 0, bytesRead);
                        Message serMsg = JsonSerializer.Deserialize<Message>(receivedMessage);
                        _recvQueue.Enqueue(serMsg, 1 /* fix it */);
                    }
                }
                if (ifAval == false)
                    Thread.Sleep(200);
            }
            Console.WriteLine("[Receiver] Receive stops");
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
                Message message = _recvQueue.Dequeue();

                // If the message is a stop message, break out of the loop
                try
                {
                    if (message.StopThread)
                        break;
                    handleMessage(message);
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

