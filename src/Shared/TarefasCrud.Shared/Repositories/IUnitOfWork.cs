namespace TarefasCrud.Shared.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}