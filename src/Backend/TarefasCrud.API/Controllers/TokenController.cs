using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Application.UseCases.Token.RefreshToken;

namespace TarefasCrud.API.Controllers;

public class TokenController : TarefasCrudControllerBase
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] CreateNewTokenCommand create)
    {
        var response = await useCase.Execute(create);
        return Ok(response);
    }
}