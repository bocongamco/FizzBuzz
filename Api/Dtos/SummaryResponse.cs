namespace FizzBuzz.Dtos;
public sealed class SummaryResponse
{
    public int ScoreCorrect { get; init; }
    public int ScoreIncorrect { get; init; }
    public int Total => ScoreCorrect + ScoreIncorrect;
}
