using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.Tasks.Register;

public class RegisterTaskUseCase : IRegisterTaskUseCase
{
    public RegisterTaskUseCase()
    {
        
    }
    public Task<ResponseRegisteredTaskJson> Execute(RequestTaskJson request)
    {
        Validate(request);
        return null;
    }

    private void Validate(RequestTaskJson request)
    {
        var validator = new TaskValidator();
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
}