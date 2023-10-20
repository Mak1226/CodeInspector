/////
/// Author: 
/////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Communicator
{
    public interface ICommunicator
    {
        /// <summary>
        /// start(server dest) -> client:
        ///     call the listen start all threads connect to server 
        /// start(server dest=null) -> server:
        ///     call the listen start all threads returns serverID
        /// </summary>
        /// <param name="destIP">Server IP when called by client</param>
        /// <param name="destPort">Server Port when called by client</param>
        /// <returns>server: Server IP, port</returns>
        public string Start(string? destIP =null, string? destPort=null);

        /// <summary>
        /// Server: Stops the server and stops all threads
        /// Client: Stops listening to the server and stops all threads
        /// </summary>
        public void Stop();

        /// <summary>
        /// Sends `serializedObj` to `destID`. We call `Send` upon happening of event `eventType`.
        /// </summary>
        /// <param name="serializedObj"></param>
        /// <param name="eventType"></param>
        /// <param name="destID"></param>
        //public void Send(object obj,string? destID=null);
        public void Send(string serializedObj, string eventType/* will be updated with a class obj (?) */, string? destID=null);
        
        //TODO: subscribe function
    }
}
