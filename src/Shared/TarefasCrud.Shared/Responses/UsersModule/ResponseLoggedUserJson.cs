namespace TarefasCrud.Shared.Responses.UsersModule;

public class ResponseLoggedUserJson
{
    public string Name { get; set; } =  string.Empty;
    public ResponseTokensJson Tokens { get; set; } =  default!;
}