namespace UsersModule.Infrastructure.Settings;

public sealed record VerificationTokenSettings
{
    public int ExpirationTimeMinutes { get; init; }
}