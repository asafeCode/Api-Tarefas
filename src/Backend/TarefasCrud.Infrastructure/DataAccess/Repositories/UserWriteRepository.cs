using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.User;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class UserWriteRepository : IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public UserWriteRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
    public async Task DeleteAccount(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null) return;
        
        var tasks = _dbContext.Tasks.Where(t => t.UserId == user.Id);
        _dbContext.Tasks.RemoveRange(tasks);
        _dbContext.Users.Remove(user);
    }
    public async Task<User> GetUserById(long id) => await _dbContext
        .Users
        .FirstAsync(user => user.Id.Equals(id) && user.Active);
    public void Update(User user) => _dbContext.Users.Update(user);
}