using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Services.Tokens;

namespace UsersModule.Application.UseCases.Token.RefreshToken;

public class CreateNewTokenHandler(
    ITokenRepository tokenRepository, 
    IUnitOfWork unitOfWork, 
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator)
{
    public async Task<ResponseTokensJson> Handle(CreateNewTokenCommand request)
    {
        var refreshToken = await tokenRepository.GetRefreshToken(request.RefreshToken);
        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();
        
        if (DateTime.Compare(refreshToken.ExpiresOn, refreshToken.CreatedOn) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = refreshTokenGenerator.CreateToken(refreshToken.UserId);
        
        await tokenRepository.AddRefreshToken(newRefreshToken);
        await unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = accessTokenGenerator.Generate(refreshToken.User.UserId),
            RefreshToken = newRefreshToken.Value
        };
    }
}