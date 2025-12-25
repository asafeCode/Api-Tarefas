namespace UsersModule.Application.UseCases.User.ChangePassword;

public record ChangePasswordCommand(ChangePasswordRequest Request);

public record ChangePasswordRequest(string Password, string NewPassword);