using TaskBurnerAPI.Data;

var builder = WebApplication.CreateBuilder(args);
var db = new TaskBurnerDB();
var app = builder.Build();

app.MapGet("/", () => "TaskBurner API running");

app.MapGet("/issues", () =>
{
    return Results.Ok(db.Issues);
});

app.MapGet("/issues/{id}", (int id) =>
{
    var issue = db.Issues.SingleOrDefault(i => i.Id == id);
    if (issue is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(issue);
});

app.Run();

public partial class Program()
{

}