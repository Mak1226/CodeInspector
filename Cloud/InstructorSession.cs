namespace Cloud
{
    public class InstructorSession
    {
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public int SessionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
