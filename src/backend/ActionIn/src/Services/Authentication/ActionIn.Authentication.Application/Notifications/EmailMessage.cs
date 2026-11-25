namespace ActionIn.Authentication.Application.Notifications;

public sealed record EmailMessage(string To, string Subject, string Body)
{
    public static EmailMessage Welcome(string to, string username) =>
        new(to, "Welcome to ActionIn!", $"Hello, {username} your account was registered! <hr> <i> Vitor Manzano Villela Dias </i>");
}


