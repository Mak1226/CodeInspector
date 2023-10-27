/////
/// Author: 
/////

using System;
using System.Diagnostics;

namespace Networking.Queues
{
	public class GenericQueue
	{
		private Queue<string> _queue;

		public GenericQueue()
		{
			this._queue = new();
		}

		public bool Enqueue(string data)
		{
			try
			{
				this._queue.Enqueue(data);
                return true;
            }
			catch (Exception ex)
			{
				Trace.WriteLine("[Queue] Exception found during enqueue");
                Trace.WriteLine($"{ex.StackTrace}");
            }
			return false;
        }

        public string Dequeue()
        {
            try
            {
                while (_queue.Count == 0) ;
                return _queue.Dequeue();
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

