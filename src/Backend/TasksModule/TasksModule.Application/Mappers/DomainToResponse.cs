

using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.SharedEntities;

namespace TasksModule.Application.Mappers;

public static class DomainToResponse
{
    public static IList<ResponseShortTaskJson> ToResponse(this IList<TaskEntity> domainList)
    {
        return domainList.Select(domain => new ResponseShortTaskJson
            {
                Id = domain.Id,
                Category = domain.Category,
                IsCompleted = domain.IsCompleted,
                Progress = domain.Progress,
                Title = domain.Title,
                WeeklyGoal = domain.WeeklyGoal,
                WeekOfMonth = domain.WeekOfMonth,
            })
            .ToList();
    }    
    public static ResponseTaskJson ToResponse(this TaskEntity domain)
    {
        return new ResponseTaskJson
        {
            Id = domain.Id,
            Description = domain.Description,
            Title = domain.Title,
            IsCompleted = domain.IsCompleted,
            WeekOfMonth = domain.WeekOfMonth,
            Category = domain.Category,
            Progress = domain.Progress,
            WeeklyGoal = domain.WeeklyGoal,
            StartDate = domain.StartDate,
        };
    }
}