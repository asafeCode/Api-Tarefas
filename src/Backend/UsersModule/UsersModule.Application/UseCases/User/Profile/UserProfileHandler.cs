using TarefasCrud.Shared.Responses.UsersModule;
using TarefasCrud.Shared.Services;
using UsersModule.Application.Mappers;

namespace UsersModule.Application.UseCases.User.Profile;

public class UserProfileHandler 
{
    private readonly ILoggedUser _loggedUser;
    public UserProfileHandler(ILoggedUser loggedUser) => _loggedUser = loggedUser;

    public async Task<ResponseUserProfileJson> Handle(UserProfileQuery query)
    {
        var user = await _loggedUser.User();
        return user.ToResponse();
    }
}