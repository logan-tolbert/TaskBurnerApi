using TaskBurnerAPI.Enums;
using TaskBurnerAPI.Models;

namespace TaskBurnerAPI.Data;

public class TaskBurnerDB
{
    public List<Issue> Issues;

    public TaskBurnerDB()
    {
        Issues =
        [
            new Issue
            {
                Id = 1,
                Title = "Sample Task 1",
                Body = "This is the first sample task.",
                Status = Status.Backlog
            },
            new Issue
            {
                Id = 2,
                Title = "Sample Task 2",
                Body = "This is the second sample task.",
                Status = Status.Ready
            }
        ];
    }

    public IEnumerable<Issue> GetIssues() => Issues;
}