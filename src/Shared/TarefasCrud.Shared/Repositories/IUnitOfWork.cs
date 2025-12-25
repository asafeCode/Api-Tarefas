namespace TarefasCrud.Core.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}