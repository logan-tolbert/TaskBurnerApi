using TaskBurnerAPI.Data;
using TaskBurnerAPI.Enums;
using TaskBurnerAPI.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("LoggingService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5142/api/logs/");
});
// TODO: Implement HttpClient connection to LogService 

// TODO: Implement API Documentation (i.e. Swagger, OpenApi)

// TODO: Add issue validation service

// TODO: Implement pagination

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

logger.LogInformation("Starting TaskBurner API\n\t{DateTime.UtcNow}", DateTime.UtcNow);

async Task SendLogToLoggingService(LogLevel logLevel, string message, IHttpClientFactory httpClientFactory)
{
    var client = httpClientFactory.CreateClient("LoggingService");
    var logMessage = new
    {
        LogLevel = logLevel,
        Message = message
    };

    try
    {
        await client.PostAsJsonAsync("log", logMessage);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to send log to LoggingService");
    }
}

app.MapGet("/", async (IHttpClientFactory httpClientFactory) =>
{
    var message = $"TaskBurnerAPI Root endpoint called:\n\t{DateTime.UtcNow}";
    logger.LogInformation(message);
    await SendLogToLoggingService(LogLevel.Information, message, httpClientFactory);
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

app.MapPost("/issues", (Issue issue) =>
{
    logger.LogInformation("Creating new issue");

    if (issue.Title == string.Empty)
    {
        logger.LogWarning("Invalid request: Title empty");
        return Results.BadRequest();
    }

    if (issue.Body == string.Empty)
    {
        logger.LogWarning("Invalid request: Body empty");
        return Results.BadRequest();
    }

    issue.Id = db.Issues.Max(i => i.Id) + 1;
    db.Issues.Add(issue);
    return Results.Created($"/issues/{issue.Id}", issue);
});

app.MapPut("/issues/{id}/status", (int id, Issue item) =>
{
    if (id <= 0)
    {
        logger.LogWarning("ID cannot be 0 or negative");
        return Results.BadRequest();
    }
    if (item.Title == string.Empty)
    {
        logger.LogWarning("Invalid request: Title empty");
        return Results.BadRequest();
    }

    if (item.Body == string.Empty)
    {
        logger.LogWarning("Invalid request: Body empty");
        return Results.BadRequest();
    }

    var issue = db.Issues.SingleOrDefault(i => i.Id == id);

    if (issue is null)
    {
        logger.LogWarning("Issue with ID {Id} not found", id);
        return Results.NotFound();
    }

    if (item.Status < Status.Backlog || item.Status > Status.Complete)
    {
        logger.LogWarning("Invalid status {Status}", item.Status);
        return Results.BadRequest("Invalid status");
    }

    issue.Status = item.Status;
    logger.LogInformation("Issue with ID {Id} updated to status {Status}", id, item.Status);
    return Results.Ok(issue);
});


app.MapDelete("/issues/{id}", (int id) =>
{
    if (id <= 0)
    {
        logger.LogWarning("Invalid ID {Id}", id);
        return Results.BadRequest("Invalid ID");
    }

    var issue = db.Issues.SingleOrDefault(i => i.Id == id);
    if (issue is null)
    {
        logger.LogWarning("Issue with ID {Id} not found", id);
        return Results.NotFound();
    }

    db.Issues.Remove(issue);
    logger.LogInformation("Issue with ID {Id} deleted", id);
    return Results.Ok(issue);
});

app.Run();

public partial class Program
{
}