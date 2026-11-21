using ActionIn.Authentication.Application.Commands.RegisterAccount;
using ActionIn.Authentication.Application.Dtos;
using ActionIn.Core.Mediatr;
using Microsoft.AspNetCore.Mvc;

namespace ActionIn.WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IMediatrHandler sender) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountDto accountDto, CancellationToken cancellationToken)
    {
        var command = new RegisterAccountCommand(accountDto.Username, accountDto.Email, accountDto.Password);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Created();

        return BadRequest();
    }
}


