using System.Linq.Expressions;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.application.iRepositories;

public interface ITeamRepository
{
    Task<List<Team>> GetAll();
    Task<Team?> GetByIdentifier(Guid identifier, CancellationToken cancellationToken);
    Task<Team?> GetTeamWithActiveChatsOnly(Guid identifier, CancellationToken cancellationToken);
    Task<List<Team>> GetTeamWorkingInTheCurrentShift(CancellationToken cancellationToken);
}