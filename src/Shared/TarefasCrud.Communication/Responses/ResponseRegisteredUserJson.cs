namespace TarefasCrud.Communication.Responses;

public class ResponseRegisteredUserJson
{
    public string Name { get; set; } =  string.Empty;
    public ResponseTokensJson Tokenses { get; set; } =  default!;
}