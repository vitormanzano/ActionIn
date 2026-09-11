using ActionIn.Authentication.Application.Dtos;

namespace ActionIn.Authentication.Application.Services;

public interface IAccountService : IDisposable
{
    Task<bool> RegisterAsync(RegisterAccountDto account);
    Task<bool> LoginAsync(LoginAccountDto account);
}
