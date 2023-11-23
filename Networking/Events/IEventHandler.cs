
using Networking.Models;
namespace Networking.Events
{
    public interface IEventHandler
    {
        public string HandleMessageRecv(Message message);
    }
}
