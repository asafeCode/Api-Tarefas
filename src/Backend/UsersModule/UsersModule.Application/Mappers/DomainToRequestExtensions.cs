using TarefasCrud.Core.Entities;
using TarefasCrud.Core.Responses.UsersModule;

namespace UsersModule.Application.Mappers;

public static class DomainToRequestExtensions
{
    public static ResponseUserProfileJson ToResponse(this User user)
    {
        return new ResponseUserProfileJson
        {
            Email = user.Email,
            Name = user.Name,
        };
    }
}