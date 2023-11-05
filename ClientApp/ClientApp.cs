using System;
using Networking;
using Networking.Communicator;

namespace ClientApp
{
	public class ClientApp
	{
        public static void Main()
        {
            Console.WriteLine("New client making");
            //var client = new Client();
            ICommunicator client = CommunicationFactory.GetCommunicator(false);
            Console.WriteLine("Starting client");
            client.Start("192.168.0.108", 12345);
            Console.WriteLine("client started");
            IEventHandler ev = new Events();
            client.Subscribe(ev, "someModule");
            client.Send("123.456.789.101", "someEvent", "server");
            Console.WriteLine("client sent");
        }
	}
}

