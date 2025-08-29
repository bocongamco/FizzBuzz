namespace FizzBuzz.Models
{
    public class Session
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int DurationSeconds { get; set; }
        public DateTimeOffset StartedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? EndedAt { get; set; }
        public int ScoreCorrect { get; set; }
        public int ScoreIncorrect { get; set; }
        public List<SessionNumber> Served { get; set; } = new();
    }
}
