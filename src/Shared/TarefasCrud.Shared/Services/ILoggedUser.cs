using TarefasCrud.Shared.SharedEntities;

namespace TarefasCrud.Shared.Services;

public interface ILoggedUser
{
    Task<User> User();
}