using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.ValueObjects;
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

    public async Task<Result> LoginAsync(string email, string password)
    {
        var emailResult = Email.Create(email);

        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        var account = await authenticationRepository.GetByEmailAsync(emailResult.Value.Address);

        if (account is null)
            return Result.Failure(AuthenticationErrors.Account.InvalidCredentials);

        var isPasswordValid = passwordHasher.Verify(password: password, hashedPassword: account.Password.Hash);

        if (!isPasswordValid)
            return Result.Failure(AuthenticationErrors.Account.InvalidCredentials);

        return Result.Success();
    }
}


