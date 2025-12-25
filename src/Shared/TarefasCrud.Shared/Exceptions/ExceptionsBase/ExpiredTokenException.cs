using System.Net;

namespace TarefasCrud.Shared.Exceptions.ExceptionsBase;

public class ExpiredTokenException : TarefasCrudException
{
    public ExpiredTokenException() : base(ResourceMessagesException.VERIFICATION_TOKEN_EXPIRED)
    {
    }
    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;

}