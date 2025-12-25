namespace TasksModule.Domain.Dtos;

public record FilterTasks(
    string? Title = null,
    string? Category = null,
    bool? IsCompleted = null,
    int? WeeklyGoalMin = null,
    int? WeeklyGoalMax = null,
    int? ProgressMin = null,
    int? ProgressMax = null,
    int? WeekOfMonth = null,
    int? Month = null,
    int? Year = null
);