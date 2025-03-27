using TaskBurnerAPI.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddDebug();
}

builder.Logging.AddEventSourceLogger();

var db = new TaskBurnerDB();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"Starting TaskBurner API\n\t{DateTime.UtcNow}");
app.MapGet("/", () =>
{
    logger.LogInformation($"Root endpoint called:\n\t{DateTime.UtcNow}");
    return "TaskBurner API v0.0.x";
});

app.MapGet("/issues", () =>
{
    logger.LogInformation("Fetching all issues");
    return Results.Ok(db.Issues);
});

app.MapGet("/issues/{id}", (int id) =>
{
    logger.LogInformation("Fetching issue with ID {Id}", id);
    var issue = db.Issues.SingleOrDefault(i => i.Id == id);
    if (issue is null)
    {
        logger.LogWarning("Issue with ID {Id} not found", id);
        return Results.NotFound();
    }
    return Results.Ok(issue);
});

app.Run();

public partial class Program
{
}