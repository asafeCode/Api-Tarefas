using TarefasCrud.Communication.Responses.UsersModule;
using TarefasCrud.Exceptions.ExceptionsBase;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Application.UseCases.Token.RefreshToken;

public class CreateNewTokenHandler 
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    
    public CreateNewTokenHandler(
        ITokenRepository tokenRepository, 
        IUnitOfWork unitOfWork, 
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    public async Task<ResponseTokensJson> Handle(CreateNewTokenCommand request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken);
        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(TarefasCrudRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.ValueObjects.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId,
        };
        
        await _tokenRepository.SaveNewRefreshToken(newRefreshToken);
        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserId),
            RefreshToken = newRefreshToken.Value
        };
    }
}