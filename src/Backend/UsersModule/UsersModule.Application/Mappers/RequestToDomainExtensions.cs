using TarefasCrud.Shared.SharedEntities;
using UsersModule.Application.UseCases.Auth.Register;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Security;

namespace UsersModule.Application.Mappers;

public static class RequestToDomainExtensions
{
    public static User ToUser(this RegisterUserCommand request, IPasswordEncripter passwordEncripter)
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = passwordEncripter.Encrypt(request.Password),
        };
    }
}