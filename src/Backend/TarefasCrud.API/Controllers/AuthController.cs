using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Shared.Responses;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Application.UseCases.Auth.Login;
using UsersModule.Application.UseCases.Auth.Register;
using UsersModule.Application.UseCases.Auth.VerifyEmail;
using Wolverine;

namespace TarefasCrud.API.Controllers;

public class AuthController : TarefasCrudControllerBase
{
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(ResponseLoggedUserJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IMessageBus sender, [FromBody] DoLoginCommand command)
    {
        var result = await sender.InvokeAsync<ResponseLoggedUserJson>(command);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, [FromServices] IMessageBus sender)
    {
        var result = await sender.InvokeAsync<ResponseRegisteredUserJson>(command);
        return Created(string.Empty, result);
    }    
    [HttpPatch]
    [Route("verify-email", Name = "VerifyEmail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> VerifyEmail([FromQuery] Guid token, [FromServices] IMessageBus mediator)
    {
        var command = new VerifyEmailCommand(token);
        await mediator.InvokeAsync(command);
        return NoContent();
    }
}