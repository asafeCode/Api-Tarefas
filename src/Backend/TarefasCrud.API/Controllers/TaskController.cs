using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Application.UseCases.Tasks;
using TarefasCrud.Application.UseCases.Tasks.Get.GetById;
using TarefasCrud.Application.UseCases.Tasks.Register;
using TarefasCrud.Application.UseCases.Tasks.Update.Task;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.API.Controllers;

[AuthenticatedUser]
public class TaskController :  TarefasCrudControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredTaskJson),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestTaskJson request,
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
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] long id,
        [FromServices] IUpdateTaskUseCase useCase,
        [FromBody]  RequestTaskJson request)
    {
        await useCase.Execute(id,  request);
        return NoContent();
    }     
    
    [HttpPut]
    [Route("{id}/progress")]
    [ProducesResponseType(typeof(ResponseTaskJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProgressClaim([FromRoute] long id,
        [FromServices] IGetTaskByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }      
    
    [HttpPut]
    [Route("{id}/progress/decrement")]
    [ProducesResponseType(typeof(ResponseTaskJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProgressUnclaim([FromRoute] long id,
        [FromServices] IGetTaskByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }   
        
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseTaskJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] long id,
        [FromServices] IGetTaskByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }   
    
    
}