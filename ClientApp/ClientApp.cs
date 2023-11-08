using System;
using Networking.Communicator;
using Networking.Events;

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
            client.Start("127.0.0.1", 12345,"A");
            Console.WriteLine("client started");
            IEventHandler ev = new Events();
            client.Subscribe(ev, "someModule");
            client.Send("123.456.789.101", "someEvent", "server");
            Console.WriteLine("client sent");
            Console.ReadKey();
            client.Stop();
            Console.ReadKey();
        }
	}
}

