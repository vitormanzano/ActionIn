using MediatR;

namespace ActionIn.Authentication.Domain.Events.NewAccountRegistered;

public class NewAccountRegisteredEventHandler : INotificationHandler<NewAccountRegisteredEvent>
{

    public async Task Handle(NewAccountRegisteredEvent message, CancellationToken cancellationToken)
    {
        // Send email to the email
        // In the future, this should happen first than the transaction, if the email is delivered
        // Then we made the transaction, by command or a query.
    }
}



