using TarefasCrud.Core.Exceptions.ExceptionsBase;
using TarefasCrud.Core.Responses.UsersModule;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Application.UseCases.Auth.Login;

public class DoLoginHandler 
{
    private readonly IUserReadOnlyRepository  _repository;
    private readonly IPasswordEncripter  _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailVerificationTokenGenerator _emailVerificationTokenGenerator;
    private readonly IEmailVerificationLinkGenerator _emailVerificationLinkGenerator;
    private readonly IEmailVerifiedPublisher _publisher;

    public DoLoginHandler(IUserReadOnlyRepository repository, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator, 
        IRefreshTokenGenerator refreshTokenGenerator, 
        ITokenRepository tokenRepository, 
        IUnitOfWork unitOfWork, 
        IEmailVerificationTokenGenerator emailVerificationTokenGenerator, 
        IEmailVerificationLinkGenerator emailVerificationLinkGenerator, 
        IEmailVerifiedPublisher publisher)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _emailVerificationTokenGenerator = emailVerificationTokenGenerator;
        _emailVerificationLinkGenerator = emailVerificationLinkGenerator;
        _publisher = publisher;
    }
    
    public async Task<ResponseLoggedUserJson> Handle(DoLoginCommand request)
    {
        var user = await _repository.GetUserByEmail(request.Email);
        if (user is null || _passwordEncripter.IsValid(request.Password, user.Password).IsFalse()) 
            throw new InvalidLoginException();

        if (user.EmailConfirmed.IsFalse())
        {
            var token = await CreateAndSaveVerificationToken(user.Id);
            var verificationLink = _emailVerificationLinkGenerator.CreateLink(token);   
            await _publisher.SendAsync(user.Email, verificationLink);
            throw new InvalidLoginException($"Account not confirmed. Verification email sent to {user.Email}.");
        }

        var refreshToken = await CreateAndSaveRefreshToken(user.Id);
        return new ResponseLoggedUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _tokenGenerator.Generate(user.UserId),
                RefreshToken = refreshToken
            }
        };
    }
    private async Task<string> CreateAndSaveRefreshToken(long userId)
    {
        var refreshToken = _refreshTokenGenerator.CreateToken(userId);
        
        await _tokenRepository.AddRefreshToken(refreshToken);

        await _unitOfWork.Commit();

        return refreshToken.Value;
    }    
    private async Task<EmailVerificationToken> CreateAndSaveVerificationToken(long userId)
    {
        var token = _emailVerificationTokenGenerator.CreateToken(userId);
        await _tokenRepository.AddVerificationToken(token);

        await _unitOfWork.Commit();

        return token;
    }
}