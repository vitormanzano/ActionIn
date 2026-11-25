using ActionIn.Authentication.Application.Notifications;
using ActionIn.Authentication.Domain.Events.NewAccountRegistered;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ActionIn.Authentication.Application.Events;

public class NewAccountRegisteredEventHandler(IEmailSender emailSender, ILogger<NewAccountRegisteredEventHandler> logger) : INotificationHandler<NewAccountRegisteredEvent>
{
    public async Task Handle(NewAccountRegisteredEvent message, CancellationToken cancellationToken)
    {
        var welcomeEmail = EmailMessage.Welcome(message.Email, message.Username);

        try
        {
            await emailSender.SendAsync(welcomeEmail, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send welcome email for account {AccountId}", message.AggregateId);
        }
    }
}


