using FluentValidation;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;

namespace UsersModule.Application.UseCases.User.Update;

public class UpdateUserHandler 
{
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser  _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUserUpdateOnlyRepository repository, 
        IUserReadOnlyRepository readOnlyRepository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUserCommand request)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(request, loggedUser.Email);
        
        var user = await _repository.GetUserById(loggedUser.Id);
        user.Name = request.Name;
        user.Email = request.Email;
        
        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(UpdateUserCommand request,  string currentEmail)
    {
        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var emailExists = await _readOnlyRepository.ExistsActiveUserWithEmail(request.Email);
            if (emailExists)
                throw new ValidationException(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
        }
    }
}