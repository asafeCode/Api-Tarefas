namespace UsersModule.Infrastructure.Settings;

public sealed record EmailVerificationSettings
{
    public int ExpirationTimeMinutes { get; init; }
}