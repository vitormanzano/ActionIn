using ActionIn.Authentication.Domain.Notifications;
using ActionIn.Authentication.Domain.ValueObjects;
using MediatR;

namespace ActionIn.Authentication.Domain.Events.NewAccountRegistered;

public class NewAccountRegisteredEventHandler(IEmailSender emailSender) : INotificationHandler<NewAccountRegisteredEvent>
{

    public async Task Handle(NewAccountRegisteredEvent message, CancellationToken cancellationToken)
    {
        var welcomeEmail = EmailMessage.Welcome(Email.Create(message.Email).Value, message.Username);

        try
        {
            await emailSender.SendAsync(welcomeEmail, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}



