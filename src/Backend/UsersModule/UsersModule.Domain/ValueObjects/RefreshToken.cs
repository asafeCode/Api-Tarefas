using UsersModule.Domain.Entities;

namespace UsersModule.Domain.ValueObjects;

public class RefreshToken : EntityBase
{
    public required string Value { get; set; } = string.Empty;
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}