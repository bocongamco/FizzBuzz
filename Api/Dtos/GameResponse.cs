namespace FizzBuzz.Dtos;
public sealed class GameResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Author { get; init; }
    public int Min { get; init; }
    public int Max { get; init; }
    public List<RuleDto> Rules { get; init; } = new();
}

public sealed class RuleDto
{
    public int Divisor { get; init; }
    public required string Word { get; init; }
    public int Order { get; init; }
}