using FizzBuzz.Models;

namespace FizzBuzz.Services;

public interface IRuleEngine
{
    string ComputeExpected(Game game, int number);
}

public class RuleEngine : IRuleEngine
{
    public string ComputeExpected(Game game, int number)
    {
        var parts = game.Rules.OrderBy(r => r.Order)
                      .Where(r => number % r.Divisor == 0)
                      .Select(r => r.Word)
                      .ToArray();
        return parts.Length == 0 ? number.ToString() : string.Concat(parts);
    }
}