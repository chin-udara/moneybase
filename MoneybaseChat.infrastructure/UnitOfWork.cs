using MoneybaseChat.application;
using MoneybaseChat.infrastructure.persistence;

namespace MoneybaseChat.infrastructure;

public class UnitOfWork(MoneybaseChatContext dBContext) : IUnitOfWork
{
    private readonly MoneybaseChatContext DBContext = dBContext;

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await DBContext.SaveChangesAsync(cancellationToken);
    }
}
