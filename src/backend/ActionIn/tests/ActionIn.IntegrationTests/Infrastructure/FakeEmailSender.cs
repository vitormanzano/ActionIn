using ActionIn.Authentication.Application.Notifications;

namespace ActionIn.IntegrationTests.Infrastructure;

public class FakeEmailSender : IEmailSender
{
    private readonly List<EmailMessage> _sentMessages = [];
    public IReadOnlyList<EmailMessage> SentMessages => _sentMessages.AsReadOnly();
    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        _sentMessages.Add(message);
        return Task.CompletedTask;
    }
}


