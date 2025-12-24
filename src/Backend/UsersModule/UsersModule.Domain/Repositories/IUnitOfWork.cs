namespace UsersModule.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}