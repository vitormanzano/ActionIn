using ActionIn.Authentication.Domain.Services;
using ActionIn.Core.Messages.Commands;

namespace ActionIn.Authentication.Application.Commands;

public class RegisterAccountCommandHandler(IAuthenticationService authenticationService) : ICommandHandler<RegisterAccountCommand>
{
    // Dummy. Should not use that!
    public async Task<bool> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        await authenticationService.RegisterAsync(command.account.Username, command.account.Email, command.account.Password);
        return true;
    }
}


