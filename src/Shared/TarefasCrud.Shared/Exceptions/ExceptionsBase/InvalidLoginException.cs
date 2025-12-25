using System.Net;

namespace TarefasCrud.Shared.Exceptions.ExceptionsBase;

public class InvalidLoginException : TarefasCrudException
{
    public InvalidLoginException(string? message = null) : base(message ?? ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID){}
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    public override IList<string> GetErrorMessages() => [Message];
}