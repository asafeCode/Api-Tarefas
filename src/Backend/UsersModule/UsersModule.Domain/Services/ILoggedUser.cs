using UsersModule.Domain.Entities;

namespace UsersModule.Domain.Services;

public interface ILoggedUser
{
    public Task<User> User();
}