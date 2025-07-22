using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MoneybaseChat.application.iRepositories;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.infrastructure.persistence.repositories;

public class TeamRepository(MoneybaseChatContext DBContext) : ITeamRepository
{
    private readonly MoneybaseChatContext DBContext = DBContext;

    public async Task<List<Team>> GetAll()
    {
        return await DBContext.Teams.ToListAsync();
    }

    public async Task<Team?> GetByIdentifier(Guid identifier, CancellationToken cancellationToken)
    {
        return await DBContext.Teams.Where(c => c.Identifier == identifier).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamWithActiveChatsOnly(Guid identifier, CancellationToken cancellationToken)
    {
        return await DBContext.Teams.Where(t => t.Identifier == identifier).Include(t => t.Agents).ThenInclude(a => a.ChatRequests.Where(a => a.IsActive)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Team>> GetTeamWorkingInTheCurrentShift(CancellationToken cancellationToken)
    {
        var requestTime = TimeOnly.FromDateTime(DateTime.Now);
        return await DBContext.Teams.Where(t => requestTime > t.ShiftStartTime &&
                requestTime <= t.ShiftStartTime.AddMinutes(t.ShiftDurationInMinutes)).ToListAsync(cancellationToken);
    }
}
