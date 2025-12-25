using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Responses;

namespace TarefasCrud.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TarefasCrudException templateException)
            HandleProjectException(templateException, context);
        
        if (context.Exception is FluentValidation.ValidationException validationException)
            HandleValidationException(validationException, context);
        else
            ThrowUnknowException(context);  
    }

    private static void HandleProjectException(TarefasCrudException tarefasCrudException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)tarefasCrudException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(tarefasCrudException.GetErrorMessages()));
    }
    private static void HandleValidationException(FluentValidation.ValidationException validationException, ExceptionContext context)
    { 
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ResponseErrorJson(validationException.Errors.Select(error => error.ErrorMessage).ToList()));
    }
    private static void ThrowUnknowException(ExceptionContext context)
    { 
         context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
         context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }


}
