using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.application.services;

public class TeamService(ITeamRepository teamRepository, IUnitOfWork unitOfWork, IQueueService queueService) : ITeamService
{
    private readonly ITeamRepository teamRepository = teamRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IQueueService queueService = queueService;

    public async Task<Guid?> Create(string name, TimeOnly shiftStartTime, int shiftDurationInMinutes, bool isOverflowTeam, CancellationToken cancellationToken)
    {
        var newTeam = new Team(Guid.NewGuid(), name, shiftStartTime, shiftDurationInMinutes, isOverflowTeam);
        // persist new team
        await unitOfWork.CommitAsync(cancellationToken);
        // public team so that a consumer can be brought up
        await queueService.PublishNewTeam(newTeam.Identifier, cancellationToken);
        return newTeam.Identifier;
    }
}
