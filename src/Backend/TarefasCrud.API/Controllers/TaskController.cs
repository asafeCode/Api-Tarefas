using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Application.UseCases.Tasks;
using TarefasCrud.Application.UseCases.Tasks.Register;
using TarefasCrud.Communication.Requests;

namespace TarefasCrud.API.Controllers;

[AuthenticatedUser]
public class TaskController :  TarefasCrudControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterTask([FromBody] RequestTaskJson request,
        [FromServices] IRegisterTaskUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}