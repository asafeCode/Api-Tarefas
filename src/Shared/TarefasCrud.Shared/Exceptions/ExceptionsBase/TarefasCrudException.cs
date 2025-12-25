using System.Net;

namespace TarefasCrud.Shared.Exceptions.ExceptionsBase;

public abstract class TarefasCrudException : SystemException
{
    protected TarefasCrudException(string messages) : base(messages) {}

    public abstract HttpStatusCode GetStatusCode();
    public abstract IList<string> GetErrorMessages();

}
