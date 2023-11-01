using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Queues
{
    public interface IQueue
    {
        /*
        enq
        deq
        */
        public void Enqueue(string data, int priority);
        public string Dequeue();
    }
}
