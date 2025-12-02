using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Application.UseCases.Tasks;
using TarefasCrud.Application.UseCases.Tasks.GetById;
using TarefasCrud.Application.UseCases.Tasks.Register;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.API.Controllers;

[AuthenticatedUser]
public class TaskController :  TarefasCrudControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredTaskJson),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterTask([FromBody] RequestTaskJson request,
        [FromServices] IRegisterTaskUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    } 
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseTaskJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] long id,
        [FromServices] IGetTaskByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }
}