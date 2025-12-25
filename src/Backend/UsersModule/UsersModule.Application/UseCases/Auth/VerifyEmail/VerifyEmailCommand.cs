namespace UsersModule.Application.UseCases.Auth.VerifyEmail;

public record VerifyEmailCommand(Guid TokenValue);