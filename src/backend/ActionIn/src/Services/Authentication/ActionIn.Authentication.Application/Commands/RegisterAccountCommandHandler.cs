using ActionIn.Authentication.Application.Services;
using ActionIn.Core.Messages.Commands;

namespace ActionIn.Authentication.Application.Commands;

public class RegisterAccountCommandHandler(AccountService accountService) : ICommandHandler<RegisterAccountCommand>
{
    // Dummy. Should not use that!
    public async Task<bool> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        accountService.RegisterAsync(command.account);
        return true;
    }
}


