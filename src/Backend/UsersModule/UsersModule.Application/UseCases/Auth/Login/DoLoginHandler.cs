using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Security;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Application.UseCases.Auth.Login;

public class DoLoginHandler 
{
    private readonly IUserReadOnlyRepository  _readRepository;
    private readonly IUserInternalRepository  _internalRepository;
    private readonly IPasswordEncripter  _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVerificationTokenGenerator _verificationTokenGenerator;
    private readonly IVerificationLinkGenerators _linkGenerators;
    private readonly IEmailPublisher _publisher;

    public DoLoginHandler(
        IUserReadOnlyRepository readRepository,
        IUserInternalRepository internalRepository, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator, 
        IRefreshTokenGenerator refreshTokenGenerator, 
        ITokenRepository tokenRepository, 
        IUnitOfWork unitOfWork, 
        IVerificationTokenGenerator verificationTokenGenerator, 
        IVerificationLinkGenerators linkGenerators, 
        IEmailPublisher publisher)
    {
        _readRepository = readRepository;
        _internalRepository = internalRepository;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _verificationTokenGenerator = verificationTokenGenerator;
        _linkGenerators = linkGenerators;
        _publisher = publisher;
    }
    
    public async Task<ResponseLoggedUserJson> Handle(DoLoginCommand request)
    {
        var user = await _internalRepository.GetUserByEmail(request.Email);
        if (user is null || _passwordEncripter.IsValid(request.Password, user.Password).IsFalse()) 
            throw new InvalidLoginException();

        if (user.Active.IsFalse()) await RecoverAccount(user);
        if (user.EmailConfirmed.IsFalse()) await VerifyAccount(user);
        
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
    private async Task VerifyAccount(TarefasCrud.Shared.SharedEntities.User user)
    {
        var token = await CreateAndSaveVerificationToken(user.Id);
        var verificationLink = _linkGenerators.CreateAccountVerificationLink(token);   
        await _publisher.SendAccountVerificationAsync(user.Email, verificationLink);
        throw new InvalidLoginException($"Account not confirmed. Verification email sent to {user.Email}.");
    }    
    private async Task RecoverAccount(TarefasCrud.Shared.SharedEntities.User user)
    {
        var token = await CreateAndSaveVerificationToken(user.Id);
        var verificationLink = _linkGenerators.CreateAccountRecoveryLink(token);   
        await _publisher.SendAccountRecoveryAsync(user.Email, verificationLink);
        throw new InvalidLoginException($"Account recovery initiated. A code has been sent to your email: {user.Email}.");
    }
    private async Task<VerificationToken> CreateAndSaveVerificationToken(long userId)
    {
        var token = _verificationTokenGenerator.CreateToken(userId);
        await _tokenRepository.AddVerificationToken(token);

        await _unitOfWork.Commit();

        return token;
    }
}