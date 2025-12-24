using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Communication.Responses.TasksModule;
using TasksModule.Application.RoutineTask.Delete;
using TasksModule.Application.RoutineTask.GetById;
using TasksModule.Application.RoutineTask.GetTasks;
using TasksModule.Application.RoutineTask.Register;
using TasksModule.Application.RoutineTask.UpdateProgress;
using TasksModule.Application.RoutineTask.UpdateTask;

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
    [Route("/tasks")]
    [ProducesResponseType(typeof(ResponseTaskJson),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTasks([FromQuery] FilterTasksDto filters,
        [FromServices] IGetTasksUseCase useCase)
    {
        var response = await useCase.Execute(filters);

        if (response.Tasks.Any())
            return Ok(response);

        return NoContent();
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
        await useCase.Execute(id, request);
        return NoContent();
    }     
    
    [HttpPatch]
    [Route("{id}/progress")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProgress([FromRoute] long id,
        [FromServices] IUpdateTaskProgressUseCase useCase,
        [FromQuery] ConfirmationOptions options)
    {
        await useCase.ExecuteIncrement(id, options.Force);
        return NoContent();
    }     
    
    [HttpPatch]
    [Route("{id}/progress/decrement")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProgressDecrement([FromRoute] long id,
        [FromServices] IUpdateTaskProgressUseCase useCase)
    {
        await useCase.ExecuteDecrement(id);
        return NoContent();
    }   
        
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] long id,
        [FromServices] IDeleteTaskUseCase useCase)
    {
        await useCase.Execute(id);
        return NoContent();
    } 
    
    
}