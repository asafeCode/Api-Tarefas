using Moq;
using OpenAI.Responses;
using TarefasCrud.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories.Tokens;

public static class TokenRepositoryBuilder
{
    public static ITokenRepository Build() => new Mock<ITokenRepository>().Object;
}