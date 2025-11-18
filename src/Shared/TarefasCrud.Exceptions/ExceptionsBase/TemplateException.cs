using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public abstract class TemplateException : SystemException
{
    protected TemplateException(string messages) : base(messages) {}

    public abstract HttpStatusCode GetStatusCode();
    public abstract IList<string> GetErrorMessage();

}
