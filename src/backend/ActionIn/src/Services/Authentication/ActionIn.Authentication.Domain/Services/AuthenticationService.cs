using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.Services;

public class AuthenticationService(IAuthenticationRepository authenticationRepository, IPasswordHasher passwordHasher) : IAuthenticationService
{
    public async Task<Account> RegisterAsync(string username, string email, string password)
    {
        Result<Account> result = Account.Register(username, email, password, passwordHasher);

        // Will be replaced for return fielderrors
        if (result.IsFailure)
            throw new Exception(ToMessage(result.Error));

        Account account = result.Value;

        Account? emailOwner = await authenticationRepository.GetByEmailAsync(account.Email.Address);
        if (emailOwner is not null)
            throw new Exception("Email already exists");

        Account? usernameOwner = await authenticationRepository.GetByUsernameAsync(account.Username.Text);
        if (usernameOwner is not null)
            throw new Exception("Username already exists");

        return account;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var existingAccount = await authenticationRepository.GetByEmailAsync(email);

        if (existingAccount is null)
            throw new Exception("Wrong credentials!");

        var isPasswordValid = passwordHasher.Verify(password: password, hashedPassword: existingAccount.Password.Hash);

        if (!isPasswordValid)
            throw new Exception("Wrong credentials!");

        return true;
    }

    private static string ToMessage(Error error)
        => error.FieldErrors.Count == 0
        ? error.Message
        : string.Join(" ", error.FieldErrors.Select(f => f.Message));
}


