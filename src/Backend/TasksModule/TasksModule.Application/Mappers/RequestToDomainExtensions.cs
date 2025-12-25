using TarefasCrud.Shared.SharedEntities;
using TasksModule.Application.UseCases.Create;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.ValueObjects;

namespace TasksModule.Application.Mappers;

public static class RequestToDomainExtensions
{
    public static TaskEntity ToTask(this CreateTaskCommand request, long userId)
    {
        return new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            UserId = userId,
            Category = request.Category,
            Progress = TarefasCrudRuleConstants.INITIAL_PROGRESS,
            WeeklyGoal = request.WeeklyGoal,
            StartDate = request.StartDate,
            WeekOfMonth = request.StartDate.GetMonthWeek(),
        };
    }
}