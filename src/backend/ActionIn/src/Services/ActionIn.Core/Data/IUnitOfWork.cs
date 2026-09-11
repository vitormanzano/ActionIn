namespace ActionIn.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
