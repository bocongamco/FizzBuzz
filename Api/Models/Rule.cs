namespace FizzBuzz.Models
{
    public class Rule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int Divisor { get; set; }            // > 1
        public required string Word { get; set; }
        public int Order { get; set; }
    }
}
