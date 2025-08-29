namespace FizzBuzz.Dtos;

public sealed class SubmitAnswerRequest
{
    public int Number { get; set; }
    public string Answer { get; set; } = string.Empty; // what player typed
}

public sealed class SubmitAnswerResponse
{
    public int Number { get; init; }                 
    public string Submitted { get; init; } = "";
    public bool IsCorrect { get; init; }
    public string Expected { get; init; } = string.Empty;

    public int ScoreCorrect { get; init; }
    public int ScoreIncorrect { get; init; }
}