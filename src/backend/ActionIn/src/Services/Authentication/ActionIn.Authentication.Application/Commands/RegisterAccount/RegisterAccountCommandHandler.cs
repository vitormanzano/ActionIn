using ActionIn.Authentication.Domain.Events.NewAccountRegistered;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.Services;
using ActionIn.Core.Mediatr;
using ActionIn.Core.Messages.Commands;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Application.Commands.RegisterAccount;

public class RegisterAccountCommandHandler(IAuthenticationService authenticationService, IAuthenticationRepository authenticationRepository, IMediatrHandler bus) : ICommandHandler<RegisterAccountCommand>
{
    public async Task<Result> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        var entity = await authenticationService.RegisterAsync(command.Username, command.Email, command.Password);

        if (entity.IsFailure)
            return Result.Failure(entity.Error);

        var account = entity.Value;

        authenticationRepository.Register(account);
        await authenticationRepository.UnitOfWork.Commit(cancellationToken);

        // That event, will be throw by the entity later
        await bus.PublishEvent(new NewAccountRegisteredEvent(account.Id, account.Email.Address, account.Username.Text));
        return Result.Success();
    }
}


