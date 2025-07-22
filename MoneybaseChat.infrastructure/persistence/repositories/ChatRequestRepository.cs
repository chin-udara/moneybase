using Microsoft.EntityFrameworkCore;
using MoneybaseChat.application.iRepositories;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.infrastructure.persistence.repositories;

public class ChatRequestRepository(MoneybaseChatContext DBContext) : IChatRequestRepository
{
    private readonly MoneybaseChatContext DBContext = DBContext;

    public async Task Add(ChatRequest chatRequest, CancellationToken cancellationToken)
    {
        await DBContext.AddAsync(chatRequest, cancellationToken);
    }

    public async Task<ChatRequest?> GetByIdentifier(Guid identifier, CancellationToken cancellationToken)
    {
        return await DBContext.ChatRequests.Where(c => c.Identifier == identifier).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task MarkInactiveChats(CancellationToken cancellationToken)
    {
        await DBContext.ChatRequests.Where(e => e.IsActive && ((e.LastPulse != null && EF.Functions.DateDiffSecond(e.LastPulse, DateTime.Now) > 3) || (e.LastPulse == null && EF.Functions.DateDiffSecond(e.CreatedOn, DateTime.Now) > 3))).ExecuteUpdateAsync(d => d.SetProperty(c => c.IsActive, false), cancellationToken);
    }
}