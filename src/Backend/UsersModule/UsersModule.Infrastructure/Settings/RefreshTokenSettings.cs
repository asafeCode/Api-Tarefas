namespace UsersModule.Infrastructure.Settings;

public sealed record RefreshTokenSettings
{
    public int ExpirationTimeDays { get; init; }
}