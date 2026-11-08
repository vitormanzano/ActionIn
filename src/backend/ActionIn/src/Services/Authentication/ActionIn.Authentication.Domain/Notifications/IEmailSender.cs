namespace ActionIn.Authentication.Domain.Notifications;

public interface IEmailSender
{
    Task SendAsync(EmailMessage email);
}


