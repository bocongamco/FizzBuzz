using FizzBuzz.Data;
using FizzBuzz.Dtos;
using FizzBuzz.Models;
using FizzBuzz.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IRuleEngine _ruleEngine;
    private readonly IRandomNumberService _random;

    public SessionController(AppDbContext db, IRuleEngine ruleEngine, IRandomNumberService random)
    {
        _db = db;
        _ruleEngine = ruleEngine;
        _random = random;
    }

    // Start a new session
    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionRequest req)
    {
        var game = await _db.Games.Include(g => g.Rules).FirstOrDefaultAsync(g => g.Id == req.GameId);
        if (game is null) return NotFound("Game not found.");

        var session = new Session
        {
            GameId = game.Id,
            DurationSeconds = req.DurationSeconds,
        };

        _db.Sessions.Add(session);
        await _db.SaveChangesAsync();

        // Get the first random number
        var number = await _random.NextUniqueAsync(session);
        if (number is null) return Problem("No numbers available.");

        session.Served.Add(new SessionNumber { SessionId = session.Id, Number = number.Value });
        await _db.SaveChangesAsync();

        return Ok(new StartSessionResponse
        {
            SessionId = session.Id,
            FirstNumber = number.Value,
            EndsAt = session.StartedAt.AddSeconds(req.DurationSeconds)
        });
    }

    // Submit an answer
    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id, [FromBody] SubmitAnswerRequest req)
    {
        var session = await _db.Sessions.Include(s => s.Game).ThenInclude(g => g.Rules)
                                        .FirstOrDefaultAsync(s => s.Id == id);
        if (session is null) return NotFound("Session not found.");
        if (session.EndedAt is not null && session.EndedAt < DateTimeOffset.UtcNow)
            return StatusCode(410, "Session expired.");

        // check if number was actually served
        var served = await _db.SessionNumbers.AnyAsync(sn => sn.SessionId == id && sn.Number == req.Number);
        if (!served) return BadRequest("Number not part of this session.");

        var expected = _ruleEngine.ComputeExpected(session.Game, req.Number);
        var isCorrect = string.Equals(req.Answer.Trim(), expected, StringComparison.OrdinalIgnoreCase);

        if (isCorrect) session.ScoreCorrect++;
        else session.ScoreIncorrect++;

        _db.Responses.Add(new Response
        {
            SessionId = id,
            Number = req.Number,
            Submitted = req.Answer,
            Expected = expected,
            IsCorrect = isCorrect
        });

        await _db.SaveChangesAsync();

        return Ok(new SubmitAnswerResponse
        {
            Number = req.Number,
            Submitted = req.Answer,
            Expected = expected,
            IsCorrect = isCorrect,
            ScoreCorrect = session.ScoreCorrect,
            ScoreIncorrect = session.ScoreIncorrect
        });
    }

    // Get the next number
    [HttpGet("{id:guid}/next")]
    public async Task<IActionResult> Next(Guid id)
    {
        var session = await _db.Sessions.Include(s => s.Game).FirstOrDefaultAsync(s => s.Id == id);
        if (session is null) return NotFound("Session not found.");
        if (session.EndedAt is not null && session.EndedAt < DateTimeOffset.UtcNow)
            return StatusCode(410, "Session expired.");

        var next = await _random.NextUniqueAsync(session);
        if (next is null) return NoContent();

        _db.SessionNumbers.Add(new SessionNumber { SessionId = session.Id, Number = next.Value });
        await _db.SaveChangesAsync();

        return Ok(new NextNumberResponse { Number = next.Value });
    }

    // Summary
    [HttpGet("{id:guid}/summary")]
    public async Task<IActionResult> Summary(Guid id)
    {
        var session = await _db.Sessions.FindAsync(id);
        if (session is null) return NotFound("Session not found.");

        return Ok(new SummaryResponse
        {
            ScoreCorrect = session.ScoreCorrect,
            ScoreIncorrect = session.ScoreIncorrect,
        });
    }
}
