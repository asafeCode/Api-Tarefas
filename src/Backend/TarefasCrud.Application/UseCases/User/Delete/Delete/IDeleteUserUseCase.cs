namespace TarefasCrud.Application.UseCases.User.Delete.Delete;

public interface IDeleteUserUseCase
{
    Task Execute(Guid userId);
}