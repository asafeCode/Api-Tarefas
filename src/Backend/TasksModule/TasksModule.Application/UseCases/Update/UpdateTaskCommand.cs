namespace TasksModule.Application.UseCases.Update;

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int WeeklyGoal { get; set; }
    
    public string Category { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
}

public record UpdateTaskCommand(long TaskId, UpdateTaskRequest Request);