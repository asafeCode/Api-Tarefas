using Microsoft.EntityFrameworkCore;
using TarefasCrud.Infrastructure;
using UsersModule.Domain.Entities;
using UsersModule.Domain.Repositories.User;

namespace  UsersModule.Infrastructure.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserWriteOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public UserRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext
        .Users
        .AnyAsync(user => user.Email.Equals(email) && user.Active);
    public async Task<User?> GetUserByEmail(string email) => await _dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active);
    public async Task<bool> ExistActiveUserWithIdentifier(Guid userId) => await _dbContext
        .Users
        .AnyAsync(user => user.UserId.Equals(userId) && user.Active);
    public async Task<User?> GetByUserIdentifier(Guid userId) => await _dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.UserId.Equals(userId) && user.Active);
    
    public async Task AddUserAsync(User user) => await _dbContext.Users.AddAsync(user);
    
    public async Task DeleteAccount(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null) return;
        
        var tasks = _dbContext.Tasks.Where(t => t.UserId == user.Id);
        var refreshTokens = _dbContext.RefreshTokens.Where(t => t.UserId == user.Id);
        
        _dbContext.Tasks.RemoveRange(tasks);
        _dbContext.RefreshTokens.RemoveRange(refreshTokens);
        _dbContext.Users.Remove(user);
    }
    public async Task<User> GetUserById(long id) => await _dbContext
        .Users
        .FirstAsync(user => user.Id.Equals(id) && user.Active);
    public void Update(User user) => _dbContext.Users.Update(user);
}