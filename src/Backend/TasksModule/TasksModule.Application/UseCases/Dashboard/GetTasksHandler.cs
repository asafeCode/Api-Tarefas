using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.Services;
using TasksModule.Application.Mappers;
using TasksModule.Domain.Dtos;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Dashboard;

public class GetTasksHandler 
{
    public static async Task<ResponseTasksJson> Handle(GetTasksQuery query, ITaskReadOnlyRepository readRepository, 
        ILoggedUser loggedUser)
    {
        var userLogged = await loggedUser.User();
        var tasks = await readRepository.GetTasks(userLogged, query.Filters);

        return new ResponseTasksJson
        {
            Tasks = tasks.ToResponse()
        };
    }
}