using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Models;

namespace Networking.Queues
{
    public interface IQueue
    {
        public void Enqueue(Message data, int priority);
        public Message Dequeue();
        public bool canDequeue();
        public Message? Peak();
    }
}
