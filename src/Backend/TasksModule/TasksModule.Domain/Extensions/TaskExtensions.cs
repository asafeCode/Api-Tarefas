using TasksModule.Domain.Entities;

namespace TasksModule.Domain.Extensions;

public static class TaskExtensions
{
    public static bool WasModifiedToday(this TaskEntity task, ISystemClock systemClock)
        => task.ModifiedAt.Date == systemClock.UseCaseDate.Date;

    public static bool IsInInitialProgress(this TaskEntity task)
        => task.Progress == TarefasCrudRuleConstants.INITIAL_PROGRESS;

    public static bool IsInCurrentWeek(this TaskEntity task, ISystemClock systemClock)
        => task.WeekOfMonth.Equals(systemClock.UseCaseDate.ToDateOnly().GetMonthWeek());
}