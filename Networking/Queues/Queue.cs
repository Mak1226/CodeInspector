/******************************************************************************
 * Filename    = Serialization/Queue.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = The class definition of the priority queue used by the sender and reciever.
 *****************************************************************************/

using System.Diagnostics;
using Networking.Models;
using System.Diagnostics;

namespace Networking.Queues
{
    /// <summary>
    /// The class definition of the priority queue used by the sender and reciever
    /// </summary>
    public class Queue : IQueue
    {
        private readonly PriorityQueue<Message, int> _queue;

        /// <summary>
        /// The lock that is to be held while managing shared resources to provide thread safety
        /// </summary>
        private readonly object _lock;

        /// <summary>
        /// The constructor for the priority queue
        /// </summary>
        public Queue()
        {
            _queue = new();
            _lock = new();
        }

        /// <summary>
        /// Enqueue a Message with a given priority.
        /// </summary>
        /// <param name="data">The Message to be enqueued</param>
        /// <param name="priority">The priority of <paramref name="data"/> to be enqueued</param>
        public void Enqueue(Message data, int priority)
        {
            lock (_lock)
            {
                _queue.Enqueue(data, priority);
            }
        }

        /// <summary>
        /// Removes and returns the element following its priority.
        /// </summary>
        /// <returns>The element with highest priority</returns>
        public Message Dequeue()
        {
            Message val = new();
            try
            {
                lock (_lock)
                {
                    val = _queue.Dequeue();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[Queue] Can not dequeue: "+ex.Message);
            }
            return val;
        }

        /// <summary>
        /// Whether the priority queue can be dequeued. This method does not dequeue an element.
        /// </summary>
        /// <returns>True if the priority queue can be dequeued</returns>
        public bool canDequeue()
        {
            return _queue.TryPeek(out Message? _, out int _);
        }

        /// <summary>
        /// Returns the next-to-be-dequeued element if the priority queue is not empty.
        /// </summary>
        /// <returns>The next-to-be-dequeued element if the priority queue is not empty. null otherwise.</returns>
        public Message? Peek()
        {
            _queue.TryPeek(out Message? message, out int _);
            return message;
        }
    }
}
