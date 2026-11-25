using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.Services;
using ActionIn.Core.Messages.Commands;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Application.Commands.RegisterAccount;

public class RegisterAccountCommandHandler(IAuthenticationService authenticationService, IAuthenticationRepository authenticationRepository) : ICommandHandler<RegisterAccountCommand>
{
    public async Task<Result> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        var registerResult = await authenticationService.RegisterAsync(command.Username, command.Email, command.Password);

        if (registerResult.IsFailure)
            return Result.Failure(registerResult.Error);

        var account = registerResult.Value;

        authenticationRepository.Register(account);
        var commitResult = await authenticationRepository.UnitOfWork.Commit(cancellationToken);

        if (commitResult.IsFailure)
            return commitResult;

        return Result.Success();
    }
}


