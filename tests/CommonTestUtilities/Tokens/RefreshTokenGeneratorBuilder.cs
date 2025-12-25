using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Infrastructure.Security.Tokens.Refresh;
using UsersModule.Domain.Services.Tokens;

namespace CommonTestUtilities.Tokens;

public static class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}