using TasksModule.Domain.Dtos;

namespace TasksModule.Application.UseCases.Dashboard;

public record GetTasksQuery(FilterTasks Filters);