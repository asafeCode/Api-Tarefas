using UsersModule.Domain.Services.Tokens;

namespace TarefasCrud.API.Token;

public class HttpContextTokenValue(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    public string Value()
    {
        var authentication = contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authentication["Bearer ".Length..].Trim();
    }
}