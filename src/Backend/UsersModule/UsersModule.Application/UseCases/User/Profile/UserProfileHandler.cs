using TarefasCrud.Communication.Responses.UsersModule;
using TarefasCrud.Core.Responses.UsersModule;
using UsersModule.Domain.Services;

namespace UsersModule.Application.UseCases.User.Profile;

public class UserProfileHandler 
{
    private readonly ILoggedUser _loggedUser;
    public UserProfileHandler(ILoggedUser loggedUser) => _loggedUser = loggedUser;

    public async Task<ResponseUserProfileJson> Handle()
    {
        var user = await _loggedUser.User();
        return null;
    }
}