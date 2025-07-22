namespace MoneybaseChat.application.iServices;

public interface IQueueService
{
    Task<bool> Enqueue(Guid TeamIdentifier, Guid chatRequestIdentifier, CancellationToken cancellationToken);
    Task<uint> GetQueueLength(Guid identifier, CancellationToken cancellationToken);
    Task PublishNewTeam(Guid teamIdentifier, CancellationToken cancellationToken);
}