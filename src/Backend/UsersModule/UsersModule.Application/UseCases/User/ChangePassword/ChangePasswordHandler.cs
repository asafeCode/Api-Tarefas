using FluentValidation;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Security;

namespace UsersModule.Application.UseCases.User.ChangePassword;

public class ChangePasswordHandler( 
    IPasswordEncripter passwordEncripter, 
    IUserUpdateOnlyRepository repository, 
    ILoggedUser loggedUser, 
    IUnitOfWork unitOfWork)
{
    public async Task Handle(ChangePasswordCommand command)
    {
        var request = command.Request;
        var userLogged = await loggedUser.User();
        Validate(request, userLogged.Password);
        
        var user = await repository.GetUserById(userLogged.Id);
        user.Password = passwordEncripter.Encrypt(request.NewPassword);
        
        repository.Update(user);
        await unitOfWork.Commit();
    }

    private void Validate(ChangePasswordRequest request, string currentPassword)
    {
        if (passwordEncripter.IsValid(request.Password, currentPassword))
            throw new ValidationException(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
    }
}