using ActionIn.Core.Data;

namespace ActionIn.Authentication.Domain.Repository;

public interface IAccountRepository : IRepository<Account>
{
    void Register(Account account);
    Task<Account?> GetByUsernameAsync(string username);
    Task<Account?> GetByEmailAsync(string email);
}
