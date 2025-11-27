using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Security.Criptography;
using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository  _repository;
    private readonly IPasswordEncripter  _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    
    public DoLoginUseCase(IUserReadOnlyRepository userRepository, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator)
    {
        _repository = userRepository;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetUserByEmail(request.Email);
        if (user is null || _passwordEncripter.IsValid(request.Password, user.Password).IsFalse())
            throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _tokenGenerator.Generate(user.UserId)
            }
        };
    }
}