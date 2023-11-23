/******************************************************************************
 * Filename    = GenericEventHandler.cs
 *
 * Author      = VM Sreeram & Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = NetworkingUnitTests
 *
 * Description = Unit test for Networking Module
 *****************************************************************************/

using Networking.Events;
using Networking.Models;
using Networking.Queues;
using Networking.Utils;

namespace NetworkingUnitTests
{
    /// <summary>
    /// Represents a generic event handler that implements the <see cref="IEventHandler"/> interface.
    /// </summary>
    public class GenericEventHandler : IEventHandler
    {
        private readonly Queue _messageQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEventHandler"/> class.
        /// </summary>
        /// <param name="messageQueue">The message queue to which incoming messages will be enqueued.</param>
        public GenericEventHandler( Queue messageQueue )
        {
            _messageQueue = messageQueue;
        }

        /// <summary>
        /// Handles the received message by enqueuing it in the associated message queue.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        /// <returns>An empty string.</returns>
        public string HandleMessageRecv( Message message )
        {
            _messageQueue.Enqueue( message , Priority.GetPriority( "" ) );

            // If the message data is "Throw," throw an exception
            if (message.Data == "Throw")
            {
                throw new Exception( "Thrown" );
            }

            return "";
        }
    }

}

