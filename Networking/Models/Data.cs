namespace Networking.Models
{
    public class Data
    {
        public string EventType { get; set; }
        public string? Payload { get; set; }
        public Data()
        {
            this.EventType = "";
            this.Payload = "";
        }
        public Data(string EventType)
        {
            this.EventType = EventType;
        }
        public Data(string payload,string EventType)
        {
            this.Payload = payload;
            this.EventType = EventType;
        }
    }
}