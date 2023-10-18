using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Communicator
{
    public interface ICommunicator
    {
        public string Start(string? destIP =null, string? destPort=null);
        public void Stop();
        //public void Send(object obj,string? destID=null);
        public void Send(string serializedObj, string eventType/* will be updated with a class obj (?) */, string? destID=null);
        
        //TODO: subscribe function
    }
}
