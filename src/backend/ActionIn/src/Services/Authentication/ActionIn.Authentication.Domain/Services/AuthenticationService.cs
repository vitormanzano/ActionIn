using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.Services;

public class AuthenticationService(IAuthenticationRepository authenticationRepository, IPasswordHasher passwordHasher) : IAuthenticationService
{
    public async Task<Result<Account>> RegisterAsync(string username, string email, string password)
    {
        Result<Account> result = Account.Register(username, email, password, passwordHasher);

        // Will be replaced for return fielderrors
        if (result.IsFailure)
            return Result<Account>.Failure(result.Error);

        Account account = result.Value;

        Account? emailOwner = await authenticationRepository.GetByEmailAsync(account.Email.Address);
        if (emailOwner is not null)
            return Result<Account>.Failure(AuthenticationErrors.Email.AlreadyInUse);

        Account? usernameOwner = await authenticationRepository.GetByUsernameAsync(account.Username.Text);
        if (usernameOwner is not null)
            return Result<Account>.Failure(AuthenticationErrors.Username.AlreadyInUse);

        return Result<Account>.Success(account);
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
}


