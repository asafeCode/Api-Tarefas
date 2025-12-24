using UsersModule.Domain.Entities;

namespace UsersModule.Domain.ValueObjects;

public class EmailVerificationToken : EntityBase
{
    public DateTime ExpiresOn { get; set; }
    public Guid Token { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}