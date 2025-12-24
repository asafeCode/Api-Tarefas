using Microsoft.EntityFrameworkCore;
using TarefasCrud.Infrastructure;
using UsersModule.Domain.Repositories.EmailVerificationToken;
using UsersModule.Domain.ValueObjects;

namespace  UsersModule.Infrastructure.Repositories;

public class EmailVerificationTokenRepository : IEmailVerifyReadRepository, IEmailVerifyWriteRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public EmailVerificationTokenRepository(TarefasCrudDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<EmailVerificationToken?> Get(Guid token) => await _dbContext
        .EmailVerificationTokens
        .Include(e => e.User)
        .FirstOrDefaultAsync(verificationToken => token.Equals(verificationToken.Token));

    public async Task AddTokenAsync(EmailVerificationToken emailVerificationToken)
    { 
        var tokens = _dbContext.EmailVerificationTokens.Where(token => token.UserId == emailVerificationToken.UserId);
        _dbContext.EmailVerificationTokens.RemoveRange(tokens);
        await _dbContext.EmailVerificationTokens.AddAsync(emailVerificationToken);
    } 
}