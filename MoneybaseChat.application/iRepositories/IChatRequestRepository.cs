using MoneybaseChat.domain.entities;

namespace MoneybaseChat.application.iRepositories;

public interface IChatRequestRepository
{
    Task<ChatRequest?> GetByIdentifier(Guid identifier, CancellationToken cancellationToken);
    Task Add(ChatRequest chatRequest, CancellationToken cancellationToken);
    Task MarkInactiveChats(CancellationToken cancellationToken);
}