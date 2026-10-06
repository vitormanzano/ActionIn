using ActionIn.Authentication.Domain.Events.NewAccountRegistered;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.Services;
using ActionIn.Core.Mediatr;
using ActionIn.Core.Messages.Commands;

namespace ActionIn.Authentication.Application.Commands.RegisterAccount;

public class RegisterAccountCommandHandler(IAuthenticationService authenticationService, IAuthenticationRepository authenticationRepository, IMediatrHandler bus) : ICommandHandler<RegisterAccountCommand>
{
    public async Task<bool> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        var entity = await authenticationService.RegisterAsync(command.Account.Username, command.Account.Email, command.Account.Password);

        authenticationRepository.Register(entity);
        var success = await authenticationRepository.UnitOfWork.Commit();
        if (!success)
            throw new Exception("Something went wrong");

        // That event, will be throw by the entity later
        await bus.PublishEvent(new NewAccountRegisteredEvent(entity.Id, entity.Email.Value, entity.Username.Value));
        return true;
    }
}


