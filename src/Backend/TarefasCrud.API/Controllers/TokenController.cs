using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Application.UseCases.Token.RefreshToken;
using Wolverine;

namespace TarefasCrud.API.Controllers;

public class TokenController : TarefasCrudControllerBase
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromServices] IMessageBus mediator,
        [FromBody] CreateNewTokenCommand command)
    {
        var response = await mediator.InvokeAsync<ResponseTokensJson>(command);
        return Ok(response);
    }
}