using Microsoft.EntityFrameworkCore;
using TarefasCrud.Infrastructure;
using TarefasCrud.Infrastructure.DataAccess;
using TarefasCrud.Shared.SharedEntities;
using UsersModule.Domain.Repositories.User;

namespace  UsersModule.Infrastructure.Repositories;

public sealed class UserRepository(TarefasCrudDbContext dbContext) : IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserWriteOnlyRepository
{
    public async Task<bool> ExistsActiveUserWithEmail(string email) => await dbContext
        .Users
        .AnyAsync(user => user.Email.Equals(email) && user.Active);
    public async Task<User?> GetUserByEmail(string email) => await dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active);
    public async Task<bool> ExistActiveUserWithIdentifier(Guid userId) => await dbContext
        .Users
        .AnyAsync(user => user.UserId.Equals(userId) && user.Active);
    public async Task<User?> GetByUserIdentifier(Guid userId) => await dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.Active && user.UserId.Equals(userId));
    
    public async Task AddUserAsync(User user) => await dbContext.Users.AddAsync(user);
    
    public async Task DeleteAccount(Guid userId)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null) return;
        
        var tasks = dbContext.Tasks.Where(t => t.UserId == user.Id);
        var refreshTokens = dbContext.RefreshTokens.Where(t => t.UserId == user.Id);
        var emailTokens = dbContext.EmailVerificationTokens.Where(t => t.UserId == user.Id);
        
        dbContext.Tasks.RemoveRange(tasks);
        dbContext.EmailVerificationTokens.RemoveRange(emailTokens);
        dbContext.RefreshTokens.RemoveRange(refreshTokens);
        dbContext.Users.Remove(user);
    }
    public async Task<User> GetUserById(long id) => await dbContext
        .Users
        .FirstAsync(user => user.Id.Equals(id) && user.Active);
    public void Update(User user) => dbContext.Users.Update(user);
}