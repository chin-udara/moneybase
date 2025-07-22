using MoneybaseChat.domain.entities;

namespace MoneybaseChat.application.iServices;

public interface ITeamService
{
    Task<Guid?> Create(string name, TimeOnly shiftStartTime, int shiftDurationInMinutes, bool isOverflowTeam, CancellationToken cancellationToken);
}