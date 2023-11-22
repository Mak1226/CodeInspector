using Networking.Events;
using Networking.Models;
using Networking.Queues;
using Networking.Utils;

namespace NetworkingUnitTests
{
	public class GenericEventHandler : IEventHandler
    {
        private readonly Queue _messageQueue;
        public GenericEventHandler(Queue messageQueue )
        {
            _messageQueue = messageQueue;
        }

        public string HandleMessageRecv(Message message)
        {
            _messageQueue.Enqueue( message , Priority.GetPriority("") );
            return "";
        }
    }
}

