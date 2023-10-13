using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Communicator
{
    public interface ICommunicator
    {
        public bool Connect(string host, int port);
        public bool Disconnect();
        public string Authenticate(string username, string password);
        public void Send(string serializedData,string? destination);
    }
}
