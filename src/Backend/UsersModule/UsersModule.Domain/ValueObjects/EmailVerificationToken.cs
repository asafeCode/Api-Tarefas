using UsersModule.Domain.Entities;

namespace UsersModule.Domain.ValueObjects;

public class EmailVerificationToken : EntityBase
{
    public DateTime ExpiresOn { get; set; }
    public string Value { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}