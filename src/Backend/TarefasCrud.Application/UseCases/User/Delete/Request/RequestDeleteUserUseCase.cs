using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Services.LoggedUser;

namespace TarefasCrud.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase :  IRequestDeleteUserUseCase
{
    private readonly ILoggedUser  _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserUpdateOnlyRepository _updateRepository;
    public RequestDeleteUserUseCase(ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork, 
        IUserUpdateOnlyRepository updateRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _updateRepository = updateRepository;
    }
    public async Task Execute()
    {
        var loggedUser = await _loggedUser.User();
        var user = await _updateRepository.GetUserById(loggedUser.Id);

        user.Active = false;
        _updateRepository.Update(user);
        await _unitOfWork.Commit();
        //Bus Service await _busService.SendAsync(loggedUser)
    }
}