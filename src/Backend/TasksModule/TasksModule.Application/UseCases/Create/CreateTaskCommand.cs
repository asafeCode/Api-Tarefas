namespace TasksModule.Application.UseCases.Create;

public class CreateTaskCommand
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int WeeklyGoal { get; set; }
    
    public string Category { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
}