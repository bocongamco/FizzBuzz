namespace FizzBuzz.Models
{
    public class Response
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public int Number { get; set; }
        public required string Submitted { get; set; }
        public required string Expected { get; set; }
        public bool IsCorrect { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
