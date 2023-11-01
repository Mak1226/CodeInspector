using System;
using System.Diagnostics;

namespace Networking.Queues
{
    public class Queue : IQueue
    {
        private PriorityQueue<string, int> _queue;
        private object _lock; // Create a lock object

        public Queue()
        {
            this._queue = new();
            this._lock = new();
        }

        public void Enqueue(string data, int priority)
        {
            bool enqueued = false;
            int maxRetries = 3;
            int retries = 0;

            while (!enqueued && retries < maxRetries)
            {
                try
                {
                    lock (_lock) // Acquire lock
                    {
                        this._queue.Enqueue(data, priority);
                        enqueued = true;
                    } // Release lock
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("[Queue] Exception found during enqueue");
                    Trace.WriteLine($"{ex.StackTrace}");
                    retries++;
                }
            }

            if (!enqueued)
            {
                //TODO: get correct exc type
                Trace.WriteLine("[Queue] Unable to enqueue after retries.");
            }
        }

        public string Dequeue()
        {
            //assuming q has an element
            //TODO: check if q is empty or not
            try
            {
                string val;
                lock (_lock) // Acquire lock
                {
                    val = _queue.Dequeue();
                } // Release lock
                return val;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[Queue] Exception found");
                Trace.WriteLine($"{ex.StackTrace}");
            }
            return "";
        }
    }
}
