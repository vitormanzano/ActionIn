using ActionIn.Authentication.Domain.ValueObjects;

namespace ActionIn.Authentication.Domain.Notifications;

public sealed record EmailMessage(Email To, string Subject, string Body)
{
    public static EmailMessage Welcome(Email to, string username) =>
        new(to, "Welcome to ActionIn!", $"Hello, {username}! Your account was registered!");
}


