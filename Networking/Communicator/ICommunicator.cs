/******************************************************************************
 * Filename    = Communicator/ICommunicator.cs
 *
 * Author      = 
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = 
 *****************************************************************************/



using Networking.Events;
namespace Networking.Communicator
{
    public interface ICommunicator
    {
        /// <summary>
        /// Server: Starts listening for TCP connections and starts sending and receiving threads 
        /// Client: Connects to the server and starts sending and receiving threads 
        /// </summary>
        /// <param name="destIP">
        /// Server: null,
        /// Client: IP address of the server
        /// </param>
        /// <param name="destPort">
        /// Server: null,
        /// Client: Port number of the server
        /// </param>
        /// <param name="senderId">
        /// The unique identification of the communicator
        /// Server: "server"; can get from Utils.ID
        /// </param>
        /// <param name="moduleName">
        /// The module name of the starter. This module will be used as default module in Send.
        /// </param>
        /// <returns>
        /// If success, a string IP address of the communicator ":" port of the communicator.
        /// If failure, "failed".
        /// </returns>
        public string Start(string? destIP, int? destPort, string senderId, string moduleName);

        /// <summary>
        /// Server: Stops listening and stops all threads
        /// Client: Stops listening to the server and stops all threads, also sends message to server that the client is stopping
        /// </summary>
        public void Stop();

        /// <summary>
        /// Sends serialized data to destination. 
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent</param>
        /// <param name="moduleName">The module where the data is to be delivered</param>
        /// <param name="destId">The Id of destination communicator where the data is to be delivered</param>
        public void Send(string serializedData, string moduleName, string destId);

        /// <summary>
        /// Sends serialized data to destination. It will be sent to the module passed in the Start function. 
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent</param>
        /// <param name="destId">The Id of destination communicator where the data is to be delivered</param>
        public void Send(string serializedData, string destId);

        /// <summary>
        /// Subscribe a handler to an event
        /// </summary>
        /// <param name="eventHandler">The implemented class of the event handler </param>
        /// <param name="moduleName">The name of module to subscribe</param>
        public void Subscribe(IEventHandler eventHandler, string moduleName);
    }
}
