using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Application.UseCases.User;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.API.Controllers;

public class UserController : TarefasCrudControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson requestRegister)
    {
        var result = await useCase.Execute(requestRegister);

        return Created(string.Empty, result);
    }
}