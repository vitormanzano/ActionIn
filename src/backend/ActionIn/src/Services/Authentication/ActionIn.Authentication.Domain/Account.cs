using ActionIn.Core.DomainObjects;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Core.Results;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.Events.NewAccountRegistered;

namespace ActionIn.Authentication.Domain;

public sealed class Account : AggregateRoot
{
    public Username Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;

    private Account() { }

    private Account(Username username, Email email, Password password)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public static Result<Account> Register(string? username,
            string? email,
            string? password,
            IPasswordHasher passwordHasher)
    {
        Result<Username> usernameResult = Username.Create(username);
        Result<Email> emailResult = Email.Create(email);
        Result<Password> passwordResult = Password.Create(password, passwordHasher);

        List<FieldError> fieldErrors = [];

        if (usernameResult.IsFailure)
            fieldErrors.Add(usernameResult.Error.ForField(nameof(Username)));

        if (emailResult.IsFailure)
            fieldErrors.Add(emailResult.Error.ForField(nameof(Email)));

        if (passwordResult.IsFailure)
            fieldErrors.Add(passwordResult.Error.ForField(nameof(Password)));

        if (fieldErrors.Count > 0)
            return Result<Account>.Failure(AuthenticationErrors.Account.InvalidData(fieldErrors));

        var account = new Account(usernameResult.Value, emailResult.Value, passwordResult.Value);

        account.AddDomainEvent(new NewAccountRegisteredEvent(account.Id, account.Email.Address, account.Username.Text));
        return Result<Account>.Success(account);
    }
}
