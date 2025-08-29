namespace FizzBuzz.Models
{
    public class SessionNumber
    {
        public Guid SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public int Number { get; set; }
    }
}
