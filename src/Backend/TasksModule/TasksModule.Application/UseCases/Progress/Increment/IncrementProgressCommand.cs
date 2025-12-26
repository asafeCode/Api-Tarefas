namespace TasksModule.Application.UseCases.Progress.Increment;

public record IncrementProgressCommand(long TaskId, bool Force);