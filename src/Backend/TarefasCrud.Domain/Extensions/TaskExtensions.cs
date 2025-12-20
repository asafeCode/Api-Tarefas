using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.ValueObjects;

namespace TarefasCrud.Domain.Extensions;

public static class TaskExtensions
{
    public static bool WasModifiedToday(this TaskEntity task, IDateProvider dateProvider)
        => task.ModifiedAt.Date == dateProvider.UseCaseDate.Date;

    public static bool IsInInitialProgress(this TaskEntity task)
        => task.Progress == TarefasCrudRuleConstants.INITIAL_PROGRESS;

    public static bool IsInCurrentWeek(this TaskEntity task, IDateProvider dateProvider)
        => task.WeekOfMonth.Equals(dateProvider.UseCaseDate.ToDateOnly().GetMonthWeek());
}