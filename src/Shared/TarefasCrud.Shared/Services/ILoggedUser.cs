using UsersModule.Domain.Entities;

namespace TarefasCrud.Shared.Services;

public interface ILoggedUser
{
    Task<User> User();
}