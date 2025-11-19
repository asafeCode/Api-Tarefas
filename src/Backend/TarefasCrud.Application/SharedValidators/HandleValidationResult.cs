using FluentValidation.Results;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.SharedValidators;

public static class HandleValidationResult
{
    public static void Validate(ValidationResult result)
    {
        var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}