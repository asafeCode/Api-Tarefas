using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Communication.Responses.UsersModule;
using UsersModule.Application.UseCases.Auth.Login;
using UsersModule.Application.UseCases.Auth.Register;

namespace TarefasCrud.API.Controllers;

public class AuthController : TarefasCrudControllerBase
{
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(ResponseLoggedUserJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, 
        [FromBody] DoLoginCommand request)
    {
        var result = await useCase.Execute(request);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(ResponseLoggedUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RegisterUserCommand request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }    
    [HttpGet]
    [Route("verify-email", Name = "VerifyEmail")]
    [ProducesResponseType(typeof(ResponseLoggedUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyEmail(
        [FromServices] IVerifyEmailUseCase useCase,
        [FromQuery] Guid token)
    {
        await useCase.Execute(token);

        return Ok(string.Empty);
    }
}