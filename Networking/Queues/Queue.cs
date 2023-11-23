using System;
using System.Diagnostics;
using Networking.Models;

namespace Networking.Queues
{
    public class Queue : IQueue
    {
        private readonly PriorityQueue<Message, int> _queue;
        private readonly object _lock; // Create a lock object
        public Queue()
        {
            _queue = new();
            _lock = new();
        }

        public void Enqueue(Message data, int priority)
        {
                    lock (_lock) // Acquire lock
                    {
                        _queue.Enqueue(data, priority);
                    } // Release lock
        }

        public Message Dequeue()
        {
            //assuming q has an element
            //TODO: check if q is empty or not
            Message val = new();
            try
            {
                lock (_lock) // Acquire lock
                {
                    val = _queue.Dequeue();
                } // Release lock
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[Queue] cant deque"+ex.Message);
            }
            return val;
        }

        public bool canDequeue()
        {
            return _queue.TryPeek(out Message? _, out int _);
        }
        public Message? Peak()
        {
            _queue.TryPeek(out Message? message, out int _);
            return message;

        }
    }
}
