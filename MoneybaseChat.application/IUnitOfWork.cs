namespace MoneybaseChat.application;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}