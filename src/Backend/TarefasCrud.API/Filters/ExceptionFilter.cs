using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Responses;

namespace TarefasCrud.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case TarefasCrudException tarefasCrudException:
                HandleProjectException(tarefasCrudException, context);
                break;
            
            case FluentValidation.ValidationException validationException:
                HandleValidationException(validationException, context);
                break;
            
            default:
                ThrowUnknowException(context);
                break;
        }
    }

    private static void HandleProjectException(TarefasCrudException tarefasCrudException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)tarefasCrudException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(tarefasCrudException.GetErrorMessages()));
    }
    private static void HandleValidationException(FluentValidation.ValidationException validationException, ExceptionContext context)
    { 
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        
        var errors = 
            validationException.Errors.Any() ? 
                validationException.Errors.Select(error => error.ErrorMessage).ToList() :
                [validationException.Message];
        
        context.Result = new ObjectResult(new ResponseErrorJson(errors));
    }
    private static void ThrowUnknowException(ExceptionContext context)
    { 
         context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
         context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }


}
