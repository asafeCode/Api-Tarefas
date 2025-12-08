using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public static class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() =>
        new JwtTokenGenerator(5, "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08");
}