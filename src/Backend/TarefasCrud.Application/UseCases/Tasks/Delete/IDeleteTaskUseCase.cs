namespace TarefasCrud.Application.UseCases.Tasks.Delete;

public interface IDeleteTaskUseCase
{
    public Task Execute(long id);
}