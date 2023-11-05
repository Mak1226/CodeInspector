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
    /*
     * destination username
     * 
     * receive class-> managing the packets received
     * the map of clientId and net stream
     * sending class-> for sending the packets -> send the networkstream to write to
     * the network stream
     * the ser. data
     * have to maintain the sending q
     * in the server/client class what do we have to maintain?
     * the mapping btw clientid and net stream and client id and username
     * 
     */
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
        public string Start(string? destIP, int? destPort);

        /// <summary>
        /// Server: Stops the server and stops all threads
        /// Client: Stops listening to the server and stops all threads
        /// </summary>
        public void Stop();

        /// <summary>
        /// Sends `serializedObj` to `destID`. We call `Send` upon happening of event `eventType`.
        /// </summary>x
        /// <param name="serializedObj"></param>
        /// <param name="eventType"></param>
        /// <param name="destID"></param>
        public void Send(string serializedObj, string eventType, string destID);
        /*
         * {
         * "destid":
         * "eventType":
         * "data":"
         *              {
         *                  "prop1":,
         *                  "prop2"
         *              }
         *        "
         * }
         */

        /// <summary>
        /// The module `moduleName` gets subscribed to events implemented in `eventHandler`
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="moduleName"></param>
        public void Subscribe(IEventHandler eventHandler, string moduleName);
    }
}
