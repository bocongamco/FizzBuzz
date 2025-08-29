using FizzBuzz.Data;
using FizzBuzz.Models;
using FizzBuzz.Dtos;             // adjust if your DTOs live elsewhere
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Controllers;

[ApiController]
[Route("Games")]                 // <- explicit, so the path is /Games (singular or plural controller names won't matter)
public sealed class GamesController : ControllerBase
{
    private readonly AppDbContext _db;

    public GamesController(AppDbContext db) => _db = db;

    // GET /Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameListItem>>> List(CancellationToken ct)
    {
        var items = await _db.Games
            .AsNoTracking()
            .Select(g => new GameListItem
            {
                Id = g.Id,
                Name = g.Name,
                Author = g.Author,
                CreatedAt = g.CreatedAt, // keep as DateTimeOffset in the DTO
                Rules = _db.Rules.Count(r => r.GameId == g.Id),
                Sessions = _db.Sessions.Count(s => s.GameId == g.Id),
                BestScore = _db.Sessions.Where(s => s.GameId == g.Id)
                                        .Select(s => (int?)s.ScoreCorrect)
                                        .Max() ?? 0
            })
            .ToListAsync(ct);

        // order AFTER fetching (in-memory)
        var ordered = items.OrderByDescending(x => x.CreatedAt).ToList();
        return Ok(ordered);
    }


    // GET /Games/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GameResponse>> Get(Guid id, CancellationToken ct)
    {
        var game = await _db.Games
            .AsNoTracking()
            .Include(g => g.Rules.OrderBy(r => r.Order))
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (game is null) return NotFound();

        var dto = new GameResponse
        {
            Id = game.Id,
            Name = game.Name,
            Author = game.Author,
            Min = game.Min,
            Max = game.Max,
            Rules = game.Rules
                        .OrderBy(r => r.Order)
                        .Select(r => new RuleDto { Divisor = r.Divisor, Word = r.Word, Order = r.Order })
                        .ToList()
        };

        return Ok(dto);
    }

    // POST /Games
    [HttpPost]
    public async Task<ActionResult<GameResponse>> Create([FromBody] CreateGameRequest req, CancellationToken ct)
    {
        // ---- Validate
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Author))
            return BadRequest("Name and Author are required.");

        if (req.Min < 1 || req.Max < req.Min)
            return BadRequest("Invalid range: Min must be >= 1 and Max must be >= Min.");

        if (req.Rules is null || req.Rules.Count == 0)
            return BadRequest("Provide at least one rule.");

        if (await _db.Games.AnyAsync(g => g.Name == req.Name.Trim(), ct))
            return Conflict("Game name already exists.");

        for (int i = 0; i < req.Rules.Count; i++)
        {
            var r = req.Rules[i];
            if (r.Divisor <= 1) return BadRequest($"Rule #{i + 1}: Divisor must be > 1.");
            if (string.IsNullOrWhiteSpace(r.Word)) return BadRequest($"Rule #{i + 1}: Word is required.");
        }

        // ---- Map
        var game = new Game
        {
            Name = req.Name.Trim(),
            Author = req.Author.Trim(),
            Min = req.Min,
            Max = req.Max,
            Rules = req.Rules.Select((r, i) => new Rule
            {
                Divisor = r.Divisor,
                Word = r.Word!.Trim(),
                Order = r.Order ?? (i + 1)
            }).ToList()
        };

        try
        {
            _db.Games.Add(game);
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            // convert database/constraint errors into a readable 400
            return Problem(
                title: "Database error when creating game",
                detail: ex.InnerException?.Message ?? ex.Message,
                statusCode: 400);
        }

        var dto = new GameResponse
        {
            Id = game.Id,
            Name = game.Name,
            Author = game.Author,
            Min = game.Min,
            Max = game.Max,
            Rules = game.Rules
                        .OrderBy(r => r.Order)
                        .Select(r => new RuleDto { Divisor = r.Divisor, Word = r.Word, Order = r.Order })
                        .ToList()
        };

        return CreatedAtAction(nameof(Get), new { id = game.Id }, dto);
    }

    // DELETE /Games/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var game = await _db.Games
            .Include(g => g.Rules)
            .Include(g => g.Sessions)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (game is null) return NotFound();

        _db.Games.Remove(game);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}