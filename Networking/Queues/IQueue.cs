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
