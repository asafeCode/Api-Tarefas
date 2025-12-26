using TarefasCrud.Shared.Responses.UsersModule;
using TarefasCrud.Shared.Services;
using UsersModule.Application.Mappers;

namespace UsersModule.Application.UseCases.User.Profile;

public class UserProfileHandler 
{
    public static async Task<ResponseUserProfileJson> Handle(UserProfileQuery query, ILoggedUser loggedUser)
    {
        var user = await loggedUser.User();
        return user.ToResponse();
    }
}