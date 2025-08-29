using FizzBuzz.Data;
using FizzBuzz.Models;
using FizzBuzz.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FizzBuzz", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite("Data Source=app.db")
     .EnableDetailedErrors()
     .EnableSensitiveDataLogging()); // dev only

builder.Logging.AddConsole();

builder.Services.AddScoped<IRuleEngine, RuleEngine>();
builder.Services.AddScoped<IRandomNumberService, RandomNumberService>();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://localhost:5173", "http://localhost:8081")
     .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI( c=>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FizzBuzz v1");
    // c.RoutePrefix = string.Empty; // optional: serve UI at '/'
});
app.UseCors();
app.MapControllers();

// migrate + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    if (!await db.Games.AnyAsync())
    {
        db.Games.Add(new Game
        {
            Name = "FooBooLoo",
            Author = "System",
            Min = 1,
            Max = 10000,
            Rules = new() {
                new Rule{Divisor=7, Word="Foo", Order=1},
                new Rule{Divisor=11, Word="Boo", Order=2},
                new Rule{Divisor=103, Word="Loo", Order=3},
            }
        });
        await db.SaveChangesAsync();
    }
}

app.Run();
