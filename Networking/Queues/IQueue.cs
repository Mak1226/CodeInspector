/******************************************************************************
 * Filename    = Serialization/IQueue.cs
 *
 * Author      = Shubhang kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Interface that provides priority queue that the sender and reciever need.
 *****************************************************************************/

using Networking.Models;

namespace Networking.Queues
{
    /// <summary>
    /// Interface that provides priority queue that the sender and reciever need
    /// </summary>
    public interface IQueue
    {
        /// <summary>
        /// Enqueue a Message with a given priority.
        /// </summary>
        /// <param name="data">The Message to be enqueued</param>
        /// <param name="priority">The priority of <paramref name="data"/> to be enqueued</param>
        public void Enqueue(Message data, int priority);

        /// <summary>
        /// Removes and returns the element following its priority.
        /// </summary>
        /// <returns>The element with highest priority</returns>
        public Message Dequeue();

        /// <summary>
        /// Whether the priority queue can be dequeued. This method does not dequeue an element.
        /// </summary>
        /// <returns>True if the priority queue can be dequeued</returns>
        public bool canDequeue();

        /// <summary>
        /// Returns the next-to-be-dequeued element if the priority queue is not empty.
        /// </summary>
        /// <returns>The next-to-be-dequeued element if the priority queue is not empty. null otherwise.</returns>
        public Message? Peek();
    }
}
