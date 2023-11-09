using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Utils;

namespace Networking.Queues
{
    public interface IQueue
    {
        /*
        enq
        deq
        */
        public void Enqueue(Message data, int priority);
        public Message Dequeue();
        public bool canDequeue();
    }
}
