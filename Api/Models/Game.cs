namespace FizzBuzz.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<Rule> Rules { get; set; } = new();
        public List<Session> Sessions { get; set; } = new();

    }
}
