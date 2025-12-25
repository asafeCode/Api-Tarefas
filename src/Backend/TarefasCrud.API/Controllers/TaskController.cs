using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Shared.Responses;
using TarefasCrud.Shared.Responses.TasksModule;
using TasksModule.Application.UseCases.Create;
using TasksModule.Application.UseCases.Dashboard;
using TasksModule.Application.UseCases.Delete;
using TasksModule.Application.UseCases.Get;
using TasksModule.Application.UseCases.Progress.Decrement;
using TasksModule.Application.UseCases.Progress.Increment;
using TasksModule.Application.UseCases.Update;
using TasksModule.Domain.Dtos;
using Wolverine;

namespace TarefasCrud.API.Controllers;

[AuthenticatedUser]
public class TaskController :  TarefasCrudControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredTaskJson),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CreateTaskCommand command,
        [FromServices] IMessageBus mediator)
    {
        var response = await mediator.InvokeAsync<ResponseRegisteredTaskJson>(command);
        return Created(string.Empty, response);
    } 
    
    [HttpGet]
    [Route("/api/tasks")]
    [ProducesResponseType(typeof(ResponseTasksJson),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTasks([FromQuery] FilterTasks filters,
        [FromServices] IMessageBus mediator)
    {
        var query = new GetTasksQuery(filters);
        var response = await mediator.InvokeAsync<ResponseTasksJson>(query);

        if (response.Tasks.Any())
            return Ok(response);

        return NoContent();
    } 
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseTaskJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] long id,
        [FromServices] IMessageBus mediator)
    {
        var query = new GetTaskQuery(id);
        var response = await mediator.InvokeAsync<ResponseTaskJson>(query);

        return Ok(response);
    }      
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] long id,
        [FromServices] IMessageBus mediator,
        [FromBody] UpdateTaskRequest request)
    {
        var command = new UpdateTaskCommand(id, request);
        await mediator.InvokeAsync(command);
        return NoContent();
    }     
    
    [HttpPatch]
    [Route("{id}/progress")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProgress([FromRoute] long id,
        [FromServices] IMessageBus mediator,
        [FromQuery] ConfirmationOptions options)
    {
        var command = new IncrementProgressCommand(id, options.Force);
        await mediator.InvokeAsync(command);
        return NoContent();
    }     
    
    [HttpPatch]
    [Route("{id}/progress/decrement")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProgressDecrement([FromRoute] long id,
        [FromServices] IMessageBus mediator)
    {
        var command = new DecrementProgressCommand(id);
        await mediator.InvokeAsync(command);
        return NoContent();
    }   
        
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] long id,
        [FromServices] IMessageBus mediator)
    {
        var command = new DeleteTaskCommand(id);
        await mediator.InvokeAsync(command);
        return NoContent();
    } 
    
    
}