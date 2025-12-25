using TarefasCrud.Shared.SharedEntities;

namespace UsersModule.Domain.ValueObjects;

public class RefreshToken : EntityBase
{
    public DateTime ExpiresOn { get; init; }
    public required string Value { get; init; } = string.Empty;
    public long UserId { get; init; }
    public User User { get; init; } = null!;
}