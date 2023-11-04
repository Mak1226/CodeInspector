using Networking.Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    internal class MainServer
    {
        public static void Main(string[] args)
        {
            Server server = new();
            server.Start(null,null);
        }
    }
}
