using ActionIn.Authentication.Domain.Events.NewAccountRegistered;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Core.Mediatr;

namespace ActionIn.Authentication.Domain.Services;

public class AuthenticationService(IAuthenticationRepository authenticationRepository, IPasswordHasher passwordHasher, IMediatrHandler bus) : IAuthenticationService
{
    public async Task<Account> RegisterAsync(string username, string email, string password)
    {
        var emailAlreadyExists = await authenticationRepository.GetByEmailAsync(email);
        if (emailAlreadyExists is not null)
            throw new Exception("Email already exists");

        var usernameAlreadyExists = await authenticationRepository.GetByUsernameAsync(username);
        if (usernameAlreadyExists is not null)
            throw new Exception("Username already exists");

        Password.Validate(password);

        var hashedPassword = Password.FromHash(passwordHasher.Hash(password));

        var entity = Account.Register(username, email, hashedPassword);
        return entity;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var existingAccount = await authenticationRepository.GetByEmailAsync(email);

        if (existingAccount is null)
            throw new Exception("Wrong credentials!");

        var isPasswordValid = passwordHasher.Verify(existingAccount.Password.Value, password);

        if (!isPasswordValid)
            throw new Exception("Wrong credentials");

        return true;
    }

    public void Dispose()
    {
        authenticationRepository?.Dispose();
    }
}


