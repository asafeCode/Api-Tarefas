namespace UsersModule.Application.UseCases.Token.RefreshToken;
public class CreateNewTokenCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}