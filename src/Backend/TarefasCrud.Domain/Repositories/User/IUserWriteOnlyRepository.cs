namespace TarefasCrud.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    public Task Add(Entities.User user);
    public Task DeleteAccount(Guid userId);
}