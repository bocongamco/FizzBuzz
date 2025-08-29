namespace FizzBuzz.Dtos;

public sealed class GameListItem
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Author { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public int Rules { get; init; }
    public int Sessions { get; init; }
    public int BestScore { get; init; }  // max ScoreCorrect among sessions
}
