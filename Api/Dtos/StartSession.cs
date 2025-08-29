namespace FizzBuzz.Dtos;
public sealed class StartSessionRequest
{
    public Guid GameId { get; set; }
    public int DurationSeconds { get; set; }   // > 0
}

public sealed class StartSessionResponse
{
    public Guid SessionId { get; init; }
    public DateTimeOffset EndsAt { get; init; }
    public int FirstNumber { get; init; }
}