using System.Security.Cryptography;
using FizzBuzz.Data;
using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Services;

public interface IRandomNumberService
{
    Task<int?> NextUniqueAsync(Session session, CancellationToken ct = default);
}

public class RandomNumberService(AppDbContext db) : IRandomNumberService
{
    public async Task<int?> NextUniqueAsync(Session session, CancellationToken ct = default)
    {
        var total = session.Game.Max - session.Game.Min + 1;
        var served = await db.SessionNumbers.CountAsync(x => x.SessionId == session.Id, ct);
        if (served >= total) return null;

        while (true)
        {
            var n = RandomNumberGenerator.GetInt32(session.Game.Min, session.Game.Max + 1);
            var exists = await db.SessionNumbers.AnyAsync(x => x.SessionId == session.Id && x.Number == n, ct);
            if (!exists) return n;
        }
    }
}
