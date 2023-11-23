namespace Networking.Models
{
    public class Data
    {
        public string EventType { get; set; }
        public string? Payload { get; set; }
        public Data()
        {
            EventType = "";
            Payload = "";
        }
        public Data(string EventType)
        {
            this.EventType = EventType;
        }
        public Data(string payload,string EventType)
        {
            Payload = payload;
            this.EventType = EventType;
        }
    }
}
