using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Shared.Responses;
using TarefasCrud.Shared.Responses.UsersModule;
using TasksModule.Domain.Dtos;
using UsersModule.Application.UseCases.User.ChangePassword;
using UsersModule.Application.UseCases.User.Delete;
using UsersModule.Application.UseCases.User.Profile;
using UsersModule.Application.UseCases.User.Update;
using Wolverine;

namespace TarefasCrud.API.Controllers;

[AuthenticatedUser]
public class UserController : TarefasCrudControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseLoggedUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile([FromServices] IMessageBus mediator)
    {
        var query = new UserProfileQuery();
        var result = await mediator.InvokeAsync<ResponseUserProfileJson>(query);
        return Ok(result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromServices] IMessageBus mediator,
        [FromBody] UpdateUserCommand command)
    {
        await mediator.InvokeAsync(command);
        return NoContent();
    } 
    
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromServices] IMessageBus mediator,
        [FromBody] ChangePasswordCommand command)
    {
        await mediator.InvokeAsync(command);
        return NoContent();
    }  
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteUser([FromServices] IMessageBus mediator, 
        [FromQuery] ConfirmationOptions options)
    {
        var command = new RequestDeleteUserCommand(options.Force); 
        await mediator.InvokeAsync(command);
        return NoContent();
    }
}