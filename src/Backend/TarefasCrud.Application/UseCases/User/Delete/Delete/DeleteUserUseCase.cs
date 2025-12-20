using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.User;

namespace TarefasCrud.Application.UseCases.User.Delete.Delete;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteUserUseCase(IUserWriteOnlyRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(Guid userId)
    {
        await _repository.DeleteAccount(userId);
        await _unitOfWork.Commit();
    }
}