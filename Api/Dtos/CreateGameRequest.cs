namespace FizzBuzz.Dtos;

public sealed class CreateGameRequest
{
    public required string Name { get; set; }
    public required string Author { get; set; }

    // Keep these since your models use Min/Max
    public int Min { get; set; }
    public int Max { get; set; }

    public List<CreateRuleDto> Rules { get; set; } = new();
}

public sealed class CreateRuleDto
{
    public int Divisor { get; set; }        // > 1
    public required string Word { get; set; }
    public int? Order { get; set; }         // optional; default to index+1 when saving
}