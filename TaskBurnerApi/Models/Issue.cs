using TaskBurnerAPI.Enums;

namespace TaskBurnerAPI.Models;

public class Issue
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }

    // Backlog, Ready, InProgress, Complete,
    public Status Status { get; set; } = Status.Backlog;

    // TODO: Future Implementation
    // public DateTime CreatedTimeStamp { get; set; }
    // public DateTime? UpdatedTimeStamp { get; set; }
    // public DateTime? CompletedTimeStamp { get; set; }
}
