using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Tokens;

namespace UsersModule.Infrastructure.Services.LoggedUser;
public class LoggedUser(
    IUserReadOnlyRepository repository, 
    ITokenProvider token, 
    IAccessTokenValidator tokenValidator
    ) : ILoggedUser
{
    public async Task<User> User()
    {
        var tokenJwt = token.Value();
        var userId = tokenValidator.ValidateAndGetUserId(tokenJwt);
        var loggedUser = await repository.GetByUserIdentifier(userId);
        return loggedUser!;
    }
}