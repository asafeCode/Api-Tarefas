using TarefasCrud.Shared.SharedEntities;

namespace UsersModule.Domain.ValueObjects;

public class EmailVerificationToken : EntityBase
{
    public DateTime ExpiresOn { get; init; }
    public Guid Value { get; init; }
    public long UserId { get; init; }
    public User User { get; init; } = null!;
}