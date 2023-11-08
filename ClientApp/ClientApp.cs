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
            var client = new Client();
            Console.WriteLine("Starting client");
            client.Start("0.0.0.0", 12345);
            Console.WriteLine("client started");
            //client.Send("123.456.789.101", "someEvent", "23");
            //Console.WriteLine("client sent");
            IEventHandler ev = new Events();
            client.Subscribe(ev, "someModule");
        }
	}
}

