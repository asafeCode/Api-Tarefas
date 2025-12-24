using Microsoft.AspNetCore.Mvc;
using TarefasCrud.API.Attributes;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Communication.Responses.UsersModule;
using TasksModule.Domain.Dtos;
using UsersModule.Application.UseCases.User.ChangePassword;
using UsersModule.Application.UseCases.User.Update;

namespace TarefasCrud.API.Controllers;

[AuthenticatedUser]
public class UserController : TarefasCrudControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseLoggedUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromServices] IUpdateUserUseCase useCase,
        [FromBody] UpdateUserCommand request)
    {
        await useCase.Execute(request);
        return NoContent();
    } 
    
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase,
        [FromBody] ChangePasswordCommand request)
    {
        await useCase.Execute(request);
        return NoContent();
    }  
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangePassword([FromServices] IRequestDeleteUserUseCase useCase, 
        [FromQuery] ConfirmationOptions options)
    {
        await useCase.Execute(options.Force);
        return NoContent();
    }
}