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
        var command = new RegisterAccountCommand(accountDto);

        var result = await sender.SendCommand(command, cancellationToken);

        if (result)
            return Created();

        return BadRequest();
    }

}


